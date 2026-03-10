using Adventure.Domain.Enums;

namespace Adventure.Domain.Entities;

public class Pet
{
    public Guid Id { get; private set; }
    public Guid CharacterId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public int HitPoints { get; private set; }
    public int MaxHitPoints { get; private set; }
    public int ArmorClass { get; private set; }
    public string AttackDice { get; private set; } = string.Empty;
    public DamageType DamageType { get; private set; }
    public bool IsActive { get; private set; }

    protected Pet() { }
}
