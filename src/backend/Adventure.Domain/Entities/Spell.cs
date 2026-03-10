using Adventure.Domain.Enums;

namespace Adventure.Domain.Entities;

public class Spell
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public int SpellLevel { get; private set; }
    public SpellSchool School { get; private set; }
    public string CastingTime { get; private set; } = string.Empty;
    public string Range { get; private set; } = string.Empty;
    public string Duration { get; private set; } = string.Empty;
    public string? DamageDice { get; private set; }
    public DamageType? DamageType { get; private set; }
    public string? HealingDice { get; private set; }
    public AbilityType? SavingThrowAbility { get; private set; }
    public CharacterClass RequiredClass { get; private set; }

    protected Spell() { }
}
