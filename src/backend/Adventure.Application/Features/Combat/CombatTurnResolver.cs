using Adventure.Domain.Combat;
using Adventure.Domain.Enums;
using Adventure.Domain.Interfaces;
using Adventure.Domain.ValueObjects;

namespace Adventure.Application.Features.Combat;

public class CombatTurnResolver
{
    private readonly ICombatEncounterStore _encounterStore;
    private readonly IRepository<Domain.Entities.Character> _characterRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDiceService _dice;
    private readonly ICombatAI _combatAI;

    public CombatTurnResolver(
        ICombatEncounterStore encounterStore,
        IRepository<Domain.Entities.Character> characterRepo,
        IUnitOfWork unitOfWork,
        IDiceService dice,
        ICombatAI combatAI)
    {
        _encounterStore = encounterStore;
        _characterRepo = characterRepo;
        _unitOfWork = unitOfWork;
        _dice = dice;
        _combatAI = combatAI;
    }

    public void ResolveMonsterTurns(CombatEncounter encounter)
    {
        while (!encounter.IsComplete && encounter.ActiveParticipant.ParticipantType == ParticipantType.Enemy)
        {
            var monster = encounter.ActiveParticipant;
            if (!monster.IsAlive)
            {
                encounter.AdvanceTurn();
                continue;
            }

            var participants = encounter.Participants
                .Select(p => new CombatParticipantInfo(
                    p.Id, p.Name, p.ParticipantType,
                    p.CurrentHp, p.MaxHp, p.ArmorClass,
                    p.AiStrategy ?? AIStrategy.Aggressive))
                .ToList();

            var action = _combatAI.DecideAction(monster.Id, participants);

            switch (action)
            {
                case CombatActionType.Attack:
                    ResolveMonsterAttack(encounter, monster);
                    break;
                default:
                    ResolveMonsterAttack(encounter, monster);
                    break;
            }

            CheckCombatEnd(encounter);
            if (!encounter.IsComplete)
                encounter.AdvanceTurn();
        }
    }

    public async Task PersistCombatResultIfComplete(CombatEncounter encounter, CancellationToken ct)
    {
        if (!encounter.IsComplete)
            return;

        var playerParticipant = encounter.Participants
            .First(p => p.ParticipantType == ParticipantType.Player);

        var character = await _characterRepo.GetByIdAsync(encounter.CharacterId, ct);
        if (character is not null)
        {
            var damageTaken = character.CurrentHitPoints - playerParticipant.CurrentHp;
            if (damageTaken > 0)
                character.TakeDamage(damageTaken);

            var healingDone = playerParticipant.CurrentHp - character.CurrentHitPoints;
            if (healingDone > 0)
                character.Heal(healingDone);

            // Persist spell slot state: restore on victory, save current state on defeat
            if (encounter.Outcome == CombatOutcome.Victory)
                character.RestoreAllSpellSlots();
            else
                character.SetSpellSlotsFromList(playerParticipant.SpellSlots);

            await _unitOfWork.SaveChangesAsync(ct);
        }

        _encounterStore.Remove(encounter.CharacterId);
    }

    private void ResolveMonsterAttack(CombatEncounter encounter, CombatParticipant monster)
    {
        var target = encounter.Participants
            .FirstOrDefault(p => p.ParticipantType == ParticipantType.Player && p.IsAlive);

        if (target is null)
        {
            CheckCombatEnd(encounter);
            return;
        }

        var hasAdvantage = monster.HasCondition(CombatCondition.Hidden)
                        || monster.HasCondition(CombatCondition.HasAdvantage);
        var hasDisadvantage = target.HasCondition(CombatCondition.Dodging)
                           || monster.HasCondition(CombatCondition.HasDisadvantage);

        var abilityMod = monster.GetAbilityModifier(AbilityType.Strength);
        var profBonus = monster.GetProficiencyBonus();
        var targetAC = target.GetEffectiveAC();

        var attackResult = CombatRules.ResolveAttackWithAdvantage(
            _dice, abilityMod, profBonus, targetAC, hasAdvantage, hasDisadvantage);

        // Shield spell wears off after being attacked
        target.RemoveCondition(CombatCondition.Shielded);

        monster.RemoveCondition(CombatCondition.Hidden);
        monster.RemoveCondition(CombatCondition.HasAdvantage);

        if (attackResult.IsHit)
        {
            var damageDice = DiceRoll.Parse(monster.AttackDice);
            var damage = CombatRules.RollDamage(_dice, damageDice, abilityMod, attackResult.IsCritical);
            target.TakeDamage(damage);

            var critText = attackResult.IsCritical ? "CRITICAL HIT! " : "";
            var disadvText = hasDisadvantage && !hasAdvantage ? " (with disadvantage)" : "";
            encounter.AddLogEntry(
                $"{critText}{monster.Name} hits {target.Name} for {damage} damage!{disadvText} (d20={attackResult.NaturalRoll} +{abilityMod}+{profBonus}={attackResult.TotalRoll} vs AC {targetAC})");

            if (!target.IsAlive)
                encounter.AddLogEntry($"{target.Name} has fallen!");
        }
        else
        {
            var disadvText = hasDisadvantage && !hasAdvantage ? " (with disadvantage)" : "";
            encounter.AddLogEntry(
                $"{monster.Name} misses {target.Name}.{disadvText} (d20={attackResult.NaturalRoll} +{abilityMod}+{profBonus}={attackResult.TotalRoll} vs AC {targetAC})");
        }
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
