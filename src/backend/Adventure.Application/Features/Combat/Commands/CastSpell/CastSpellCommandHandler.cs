using Adventure.Domain.Combat;
using Adventure.Domain.Enums;
using Adventure.Domain.Interfaces;
using MediatR;
using SpellEntity = Adventure.Domain.Entities.Spell;

namespace Adventure.Application.Features.Combat.Commands.CastSpell;

public class CastSpellCommandHandler : IRequestHandler<CastSpellCommand, CombatActionResultDto>
{
    private readonly ICombatEncounterStore _encounterStore;
    private readonly IRepository<SpellEntity> _spellRepo;
    private readonly IDiceService _dice;

    public CastSpellCommandHandler(
        ICombatEncounterStore encounterStore,
        IRepository<SpellEntity> spellRepo,
        IDiceService dice)
    {
        _encounterStore = encounterStore;
        _spellRepo = spellRepo;
        _dice = dice;
    }

    public async Task<CombatActionResultDto> Handle(CastSpellCommand request, CancellationToken cancellationToken)
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

        var spell = await _spellRepo.GetByIdAsync(request.SpellId, cancellationToken)
            ?? throw new KeyNotFoundException($"Spell with id {request.SpellId} not found.");

        if (!active.KnownSpellIds.Contains(spell.Id))
            throw new InvalidOperationException($"Character does not know the spell {spell.Name}.");

        // Cantrips don't consume spell slots
        if (!spell.IsCantrip)
        {
            if (!active.HasSpellSlot(spell.SpellLevel))
                throw new InvalidOperationException($"No available spell slot for level {spell.SpellLevel}.");

            active.UseSpellSlot(spell.SpellLevel);
        }

        CombatActionResultDto result;

        if (spell.IsHealingSpell)
            result = ResolveHealingSpell(encounter, active, spell, request.TargetId);
        else if (spell.DamageDice is null && spell.HealingDice is null)
            result = ResolveBuffSpell(encounter, active, spell, request.TargetId);
        else if (spell.RequiresAttackRoll)
            result = ResolveAttackSpell(encounter, active, spell, request.TargetId);
        else if (spell.SavingThrowAbility.HasValue)
            result = ResolveSavingThrowSpell(encounter, active, spell, request.TargetId);
        else
            result = ResolveAutoHitSpell(encounter, active, spell, request.TargetId);

        encounter.SetActionTaken();
        CheckCombatEnd(encounter);

        return result with { UpdatedState = encounter.ToStateDto() };
    }

    private CombatActionResultDto ResolveAttackSpell(
        CombatEncounter encounter, CombatParticipant caster, SpellEntity spell, Guid? targetId)
    {
        var target = GetEnemyTarget(encounter, targetId);
        var targetAC = target.GetEffectiveAC();

        var spellMod = caster.GetSpellcastingAbilityModifier();
        var profBonus = caster.GetProficiencyBonus();

        var attackResult = CombatRules.ResolveSpellAttack(
            _dice.RollD20(), spellMod, profBonus, targetAC);

        var damageDealt = 0;
        string description;

        if (attackResult.IsHit)
        {
            damageDealt = CombatRules.RollSpellDamage(_dice, spell.DamageDice!, attackResult.IsCritical);
            target.TakeDamage(damageDealt);

            var critText = attackResult.IsCritical ? "CRITICAL HIT! " : "";
            description = $"{critText}{caster.Name} casts {spell.Name} at {target.Name} for {damageDealt} damage! (d20={attackResult.NaturalRoll} +{spellMod}+{profBonus}={attackResult.TotalRoll} vs AC {targetAC})";

            // Guiding Bolt special: grant advantage on next attack against target
            if (spell.Name == "Guiding Bolt")
            {
                target.ApplyCondition(CombatCondition.HasDisadvantage);
                description += " The target glows with mystical light!";
            }

            if (!target.IsAlive)
                description += $" {target.Name} is defeated!";
        }
        else
        {
            description = $"{caster.Name} casts {spell.Name} at {target.Name} but misses. (d20={attackResult.NaturalRoll} +{spellMod}+{profBonus}={attackResult.TotalRoll} vs AC {targetAC})";
        }

        encounter.AddLogEntry(description);

        return new CombatActionResultDto(
            (int)CombatActionType.CastSpell,
            caster.Name, target.Name,
            attackResult.NaturalRoll, attackResult.TotalRoll, targetAC,
            attackResult.IsHit, attackResult.IsCritical,
            damageDealt, 0, description, null!);
    }

    private CombatActionResultDto ResolveSavingThrowSpell(
        CombatEncounter encounter, CombatParticipant caster, SpellEntity spell, Guid? targetId)
    {
        var target = GetEnemyTarget(encounter, targetId);
        var spellSaveDC = caster.GetSpellSaveDC();

        var saveRoll = _dice.RollD20();
        var saveMod = target.GetAbilityModifier(spell.SavingThrowAbility!.Value);
        var saved = CombatRules.ResolveSavingThrow(saveRoll, saveMod, spellSaveDC);

        var fullDamage = CombatRules.RollSpellDamage(_dice, spell.DamageDice!, false);
        var damageDealt = saved ? fullDamage / 2 : fullDamage;

        if (damageDealt > 0)
            target.TakeDamage(damageDealt);

        var saveText = saved ? "saves" : "fails";
        var halfText = saved ? " (half damage)" : "";
        var description = $"{caster.Name} casts {spell.Name}! {target.Name} {saveText} the saving throw{halfText}. {damageDealt} damage! (Save: d20={saveRoll} +{saveMod}={saveRoll + saveMod} vs DC {spellSaveDC})";

        if (!target.IsAlive)
            description += $" {target.Name} is defeated!";

        encounter.AddLogEntry(description);

        return new CombatActionResultDto(
            (int)CombatActionType.CastSpell,
            caster.Name, target.Name,
            saveRoll, saveRoll + saveMod, spellSaveDC,
            !saved, false,
            damageDealt, 0, description, null!);
    }

    private CombatActionResultDto ResolveAutoHitSpell(
        CombatEncounter encounter, CombatParticipant caster, SpellEntity spell, Guid? targetId)
    {
        var target = GetEnemyTarget(encounter, targetId);

        var damageDealt = CombatRules.RollSpellDamage(_dice, spell.DamageDice!, false);
        target.TakeDamage(damageDealt);

        var description = $"{caster.Name} casts {spell.Name} at {target.Name} for {damageDealt} damage! The spell hits unerringly.";

        if (!target.IsAlive)
            description += $" {target.Name} is defeated!";

        encounter.AddLogEntry(description);

        return new CombatActionResultDto(
            (int)CombatActionType.CastSpell,
            caster.Name, target.Name,
            null, null, null,
            true, false,
            damageDealt, 0, description, null!);
    }

    private CombatActionResultDto ResolveHealingSpell(
        CombatEncounter encounter, CombatParticipant caster, SpellEntity spell, Guid? targetId)
    {
        // Healing targets self or allies
        CombatParticipant target;
        if (targetId.HasValue)
        {
            target = encounter.GetParticipant(targetId.Value);
            if (target.ParticipantType == ParticipantType.Enemy)
                throw new InvalidOperationException("Cannot heal an enemy.");
        }
        else
        {
            target = caster; // Self-heal
        }

        if (!target.IsAlive)
            throw new InvalidOperationException($"{target.Name} is not alive.");

        var healingDone = CombatRules.RollSpellHealing(_dice, spell.HealingDice!);

        // Add spellcasting modifier to healing
        healingDone += Math.Max(0, caster.GetSpellcastingAbilityModifier());
        target.Heal(healingDone);

        var targetText = target.Id == caster.Id ? "themselves" : target.Name;
        var description = $"{caster.Name} casts {spell.Name} on {targetText}, restoring {healingDone} HP! ({target.CurrentHp}/{target.MaxHp})";

        encounter.AddLogEntry(description);

        return new CombatActionResultDto(
            (int)CombatActionType.CastSpell,
            caster.Name, target.Id == caster.Id ? null : target.Name,
            null, null, null,
            true, false,
            0, healingDone, description, null!);
    }

    private CombatActionResultDto ResolveBuffSpell(
        CombatEncounter encounter, CombatParticipant caster, SpellEntity spell, Guid? targetId)
    {
        string description;

        if (spell.Name == "Shield")
        {
            caster.ApplyCondition(CombatCondition.Shielded);
            description = $"{caster.Name} casts {spell.Name}! A magical barrier grants +5 AC until next turn.";
        }
        else if (spell.Name == "Bless")
        {
            CombatParticipant target;
            if (targetId.HasValue)
            {
                target = encounter.GetParticipant(targetId.Value);
                if (target.ParticipantType == ParticipantType.Enemy)
                    throw new InvalidOperationException("Cannot bless an enemy.");
            }
            else
            {
                target = caster;
            }

            target.ApplyCondition(CombatCondition.HasAdvantage);
            var targetText = target.Id == caster.Id ? "themselves" : target.Name;
            description = $"{caster.Name} casts {spell.Name} on {targetText}, granting advantage on the next attack!";
        }
        else
        {
            description = $"{caster.Name} casts {spell.Name}.";
        }

        encounter.AddLogEntry(description);

        return new CombatActionResultDto(
            (int)CombatActionType.CastSpell,
            caster.Name, null,
            null, null, null,
            true, false,
            0, 0, description, null!);
    }

    private static CombatParticipant GetEnemyTarget(CombatEncounter encounter, Guid? targetId)
    {
        if (!targetId.HasValue)
            throw new InvalidOperationException("A target must be selected for this spell.");

        var target = encounter.GetParticipant(targetId.Value);
        if (!target.IsAlive)
            throw new InvalidOperationException($"{target.Name} is already dead.");
        if (target.ParticipantType != ParticipantType.Enemy)
            throw new InvalidOperationException("Damage spells must target an enemy.");

        return target;
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
