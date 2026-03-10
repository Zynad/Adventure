using Adventure.Domain.Enums;

namespace Adventure.Domain.Entities;

public class Companion
{
    public Guid Id { get; private set; }
    public Guid CharacterId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public CharacterClass Class { get; private set; }
    public Race Race { get; private set; }
    public int Level { get; private set; }
    public int CurrentHitPoints { get; private set; }
    public int MaxHitPoints { get; private set; }
    public int ArmorClass { get; private set; }
    public int Strength { get; private set; }
    public int Dexterity { get; private set; }
    public int Constitution { get; private set; }
    public int Intelligence { get; private set; }
    public int Wisdom { get; private set; }
    public int Charisma { get; private set; }
    public AIStrategy AIStrategy { get; private set; }
    public bool IsActive { get; private set; }

    protected Companion() { }
}
