using Adventure.Domain.Combat;
using Adventure.Domain.Enums;
using Adventure.Domain.Interfaces;
using Adventure.Domain.ValueObjects;
using MediatR;

namespace Adventure.Application.Features.Combat.Commands.ExecuteCombatAction;

public class ExecuteCombatActionCommandHandler : IRequestHandler<ExecuteCombatActionCommand, CombatActionResultDto>
{
    private readonly ICombatEncounterStore _encounterStore;
    private readonly IDiceService _dice;

    public ExecuteCombatActionCommandHandler(
        ICombatEncounterStore encounterStore,
        IDiceService dice)
    {
        _encounterStore = encounterStore;
        _dice = dice;
    }

    public Task<CombatActionResultDto> Handle(ExecuteCombatActionCommand request, CancellationToken cancellationToken)
    {
        var encounter = _encounterStore.GetByCharacterId(request.CharacterId)
            ?? throw new KeyNotFoundException("No active combat encounter found.");

        if (encounter.IsComplete)
            throw new InvalidOperationException("Combat has already ended.");

        var active = encounter.ActiveParticipant;
        if (active.ParticipantType != ParticipantType.Player || active.Id != request.CharacterId)
            throw new InvalidOperationException("It is not the player's turn.");

        if (encounter.HasTakenAction)
            throw new InvalidOperationException("You have already taken an action this turn.");

        var result = request.ActionType switch
        {
            CombatActionType.Attack => ResolveAttack(encounter, active, request.TargetId!.Value),
            CombatActionType.Dodge => ResolveDodge(encounter, active),
            CombatActionType.Disengage => ResolveDisengage(encounter, active),
            CombatActionType.Help => ResolveHelp(encounter, active, request.TargetId!.Value),
            CombatActionType.Hide => ResolveHide(encounter, active),
            _ => throw new InvalidOperationException($"Action type {request.ActionType} is not supported.")
        };

        encounter.SetActionTaken();
        CheckCombatEnd(encounter);

        return Task.FromResult(result with { UpdatedState = encounter.ToStateDto() });
    }

    private CombatActionResultDto ResolveAttack(CombatEncounter encounter, CombatParticipant attacker, Guid targetId)
    {
        var target = encounter.GetParticipant(targetId);
        if (!target.IsAlive)
            throw new InvalidOperationException($"{target.Name} is already dead.");
        if (target.ParticipantType == ParticipantType.Player)
            throw new InvalidOperationException("Cannot attack an ally.");

        var hasAdvantage = attacker.HasCondition(CombatCondition.Hidden)
                        || attacker.HasCondition(CombatCondition.HasAdvantage);
        var hasDisadvantage = target.HasCondition(CombatCondition.Dodging)
                           || attacker.HasCondition(CombatCondition.HasDisadvantage);

        var abilityMod = attacker.GetAbilityModifier(AbilityType.Strength);
        var profBonus = attacker.GetProficiencyBonus();

        var attackResult = CombatRules.ResolveAttackWithAdvantage(
            _dice, abilityMod, profBonus, target.ArmorClass, hasAdvantage, hasDisadvantage);

        // Clear one-shot conditions after attack
        attacker.RemoveCondition(CombatCondition.Hidden);
        attacker.RemoveCondition(CombatCondition.HasAdvantage);

        var damageDealt = 0;
        string description;

        if (attackResult.IsHit)
        {
            var damageDice = DiceRoll.Parse(attacker.AttackDice);
            damageDealt = CombatRules.RollDamage(_dice, damageDice, abilityMod, attackResult.IsCritical);
            target.TakeDamage(damageDealt);

            var critText = attackResult.IsCritical ? "CRITICAL HIT! " : "";
            var advText = hasAdvantage && !hasDisadvantage ? " (with advantage)" : "";
            var disadvText = hasDisadvantage && !hasAdvantage ? " (with disadvantage)" : "";
            description = $"{critText}{attacker.Name} hits {target.Name} for {damageDealt} damage!{advText}{disadvText} (d20={attackResult.NaturalRoll} +{abilityMod}+{profBonus}={attackResult.TotalRoll} vs AC {target.ArmorClass})";

            if (!target.IsAlive)
                description += $" {target.Name} is defeated!";
        }
        else
        {
            var disadvText = hasDisadvantage && !hasAdvantage ? " (with disadvantage)" : "";
            description = $"{attacker.Name} misses {target.Name}.{disadvText} (d20={attackResult.NaturalRoll} +{abilityMod}+{profBonus}={attackResult.TotalRoll} vs AC {target.ArmorClass})";
        }

        encounter.AddLogEntry(description);

        return new CombatActionResultDto(
            (int)CombatActionType.Attack,
            attacker.Name, target.Name,
            attackResult.NaturalRoll, attackResult.TotalRoll, target.ArmorClass,
            attackResult.IsHit, attackResult.IsCritical,
            damageDealt, 0, description, null!);
    }

    private CombatActionResultDto ResolveDodge(CombatEncounter encounter, CombatParticipant participant)
    {
        var description = CombatRules.ResolveDodge(participant);
        encounter.AddLogEntry(description);

        return new CombatActionResultDto(
            (int)CombatActionType.Dodge,
            participant.Name, null,
            null, null, null,
            false, false, 0, 0, description, null!);
    }

    private CombatActionResultDto ResolveDisengage(CombatEncounter encounter, CombatParticipant participant)
    {
        var description = CombatRules.ResolveDisengage(participant);
        encounter.AddLogEntry(description);

        return new CombatActionResultDto(
            (int)CombatActionType.Disengage,
            participant.Name, null,
            null, null, null,
            false, false, 0, 0, description, null!);
    }

    private CombatActionResultDto ResolveHelp(CombatEncounter encounter, CombatParticipant helper, Guid targetId)
    {
        var ally = encounter.GetParticipant(targetId);
        if (ally.ParticipantType == ParticipantType.Enemy)
            throw new InvalidOperationException("Cannot help an enemy.");
        if (!ally.IsAlive)
            throw new InvalidOperationException($"{ally.Name} is not alive.");

        var description = CombatRules.ResolveHelp(helper, ally);
        encounter.AddLogEntry(description);

        return new CombatActionResultDto(
            (int)CombatActionType.Help,
            helper.Name, ally.Name,
            null, null, null,
            true, false, 0, 0, description, null!);
    }

    private CombatActionResultDto ResolveHide(CombatEncounter encounter, CombatParticipant hider)
    {
        var enemies = encounter.Participants
            .Where(p => p.ParticipantType == ParticipantType.Enemy)
            .ToList();

        var description = CombatRules.ResolveHide(_dice, hider, enemies);
        encounter.AddLogEntry(description);

        return new CombatActionResultDto(
            (int)CombatActionType.Hide,
            hider.Name, null,
            null, null, null,
            hider.HasCondition(CombatCondition.Hidden), false, 0, 0, description, null!);
    }

    private static void CheckCombatEnd(CombatEncounter encounter)
    {
        if (encounter.AreAllEnemiesDead())
        {
            encounter.AddLogEntry("Victory! All enemies have been defeated.");
            encounter.EndCombat(CombatOutcome.Victory);
        }
        else if (encounter.IsPlayerDead())
        {
            encounter.AddLogEntry("Defeat... You have fallen in battle.");
            encounter.EndCombat(CombatOutcome.Defeat);
        }
    }
}
