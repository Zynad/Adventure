using Adventure.Domain.Enums;
using Adventure.Domain.Interfaces;
using Adventure.Domain.ValueObjects;

namespace Adventure.Domain.Combat;

public static class CombatRules
{
    public static AttackResult ResolveAttack(int naturalD20Roll, int abilityModifier, int proficiencyBonus, int targetAC)
    {
        if (naturalD20Roll == 1)
            return new AttackResult(naturalD20Roll, naturalD20Roll + abilityModifier + proficiencyBonus, IsHit: false, IsCritical: false);

        var isCritical = naturalD20Roll == 20;
        var totalRoll = naturalD20Roll + abilityModifier + proficiencyBonus;
        var isHit = isCritical || totalRoll >= targetAC;

        return new AttackResult(naturalD20Roll, totalRoll, isHit, isCritical);
    }

    public static AttackResult ResolveAttackWithAdvantage(
        IDiceService dice, int abilityModifier, int proficiencyBonus, int targetAC,
        bool hasAdvantage, bool hasDisadvantage)
    {
        int naturalRoll;

        // D&D 5e: if both advantage and disadvantage, they cancel out — roll normally
        if (hasAdvantage == hasDisadvantage)
        {
            naturalRoll = dice.RollD20();
        }
        else
        {
            var roll1 = dice.RollD20();
            var roll2 = dice.RollD20();
            naturalRoll = hasAdvantage ? Math.Max(roll1, roll2) : Math.Min(roll1, roll2);
        }

        return ResolveAttack(naturalRoll, abilityModifier, proficiencyBonus, targetAC);
    }

    public static int RollDamage(IDiceService dice, DiceRoll damageDice, int abilityModifier, bool isCritical)
    {
        var diceToRoll = isCritical
            ? new DiceRoll(damageDice.Count * 2, damageDice.Sides)
            : damageDice;

        var damage = dice.Roll(diceToRoll) + abilityModifier;
        return Math.Max(1, damage);
    }

    public static string ResolveDodge(CombatParticipant participant)
    {
        participant.ApplyCondition(CombatCondition.Dodging);
        return $"{participant.Name} takes the Dodge action, bracing for incoming attacks.";
    }

    public static string ResolveDisengage(CombatParticipant participant)
    {
        participant.ApplyCondition(CombatCondition.Disengaging);
        return $"{participant.Name} disengages, carefully retreating from melee.";
    }

    public static string ResolveHelp(CombatParticipant helper, CombatParticipant ally)
    {
        ally.ApplyCondition(CombatCondition.HasAdvantage);
        return $"{helper.Name} helps {ally.Name}, granting advantage on their next attack.";
    }

    public static string ResolveHide(IDiceService dice, CombatParticipant hider, IReadOnlyList<CombatParticipant> enemies)
    {
        var stealthRoll = dice.RollD20();
        var stealthMod = hider.GetAbilityModifier(AbilityType.Dexterity);
        var stealthTotal = stealthRoll + stealthMod;

        var highestPerception = enemies
            .Where(e => e.IsAlive)
            .Select(e => e.GetPassivePerception())
            .DefaultIfEmpty(10)
            .Max();

        if (stealthTotal >= highestPerception)
        {
            hider.ApplyCondition(CombatCondition.Hidden);
            return $"{hider.Name} hides successfully! (Stealth: d20={stealthRoll} +{stealthMod}={stealthTotal} vs Perception {highestPerception})";
        }

        return $"{hider.Name} fails to hide. (Stealth: d20={stealthRoll} +{stealthMod}={stealthTotal} vs Perception {highestPerception})";
    }

    public static bool IsDangerousZone(ZoneType zoneType)
    {
        return zoneType is ZoneType.Forest or ZoneType.Mountain or ZoneType.Cave
            or ZoneType.Dungeon or ZoneType.Swamp or ZoneType.Desert;
    }

    public static bool ShouldTriggerEncounter(IDiceService dice)
    {
        return dice.RollD20() == 1;
    }
}
