using Adventure.Domain.Enums;

namespace Adventure.Domain.Entities;

public class Spell
{
    public Guid Id { get; private protected set; }
    public string Name { get; private protected set; } = string.Empty;
    public string Description { get; private protected set; } = string.Empty;
    public int SpellLevel { get; private protected set; }
    public SpellSchool School { get; private protected set; }
    public string CastingTime { get; private protected set; } = string.Empty;
    public string Range { get; private protected set; } = string.Empty;
    public string Duration { get; private protected set; } = string.Empty;
    public string? DamageDice { get; private protected set; }
    public DamageType? DamageType { get; private protected set; }
    public string? HealingDice { get; private protected set; }
    public AbilityType? SavingThrowAbility { get; private protected set; }
    public CharacterClass RequiredClass { get; private protected set; }
    public bool RequiresAttackRoll { get; private protected set; }

    public bool IsCantrip => SpellLevel == 0;
    public bool IsHealingSpell => HealingDice is not null;

    protected Spell() { }

    internal static Spell CreateForSeed(
        Guid id, string name, string description, int spellLevel,
        SpellSchool school, string castingTime, string range, string duration,
        string? damageDice, DamageType? damageType, string? healingDice,
        AbilityType? savingThrowAbility, CharacterClass requiredClass,
        bool requiresAttackRoll)
    {
        return new Spell
        {
            Id = id,
            Name = name,
            Description = description,
            SpellLevel = spellLevel,
            School = school,
            CastingTime = castingTime,
            Range = range,
            Duration = duration,
            DamageDice = damageDice,
            DamageType = damageType,
            HealingDice = healingDice,
            SavingThrowAbility = savingThrowAbility,
            RequiredClass = requiredClass,
            RequiresAttackRoll = requiresAttackRoll
        };
    }
}
