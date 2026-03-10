using Adventure.Domain.Enums;

namespace Adventure.Domain.Entities;

public class Monster
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public int Level { get; private set; }
    public int HitPoints { get; private set; }
    public int ArmorClass { get; private set; }
    public int Strength { get; private set; }
    public int Dexterity { get; private set; }
    public int Constitution { get; private set; }
    public int Intelligence { get; private set; }
    public int Wisdom { get; private set; }
    public int Charisma { get; private set; }
    public decimal ChallengeRating { get; private set; }
    public int ExperienceReward { get; private set; }
    public string AttackDice { get; private set; } = string.Empty;
    public DamageType DamageType { get; private set; }
    public AIStrategy AIStrategy { get; private set; }
    public Guid? LootTableId { get; private set; }

    // Navigation properties
    public LootTable? LootTable { get; private set; }

    protected Monster() { }

    internal static Monster CreateForSeed(
        Guid id, string name, string description, int level,
        int hitPoints, int armorClass,
        int str, int dex, int con, int intel, int wis, int cha,
        decimal challengeRating, int experienceReward,
        string attackDice, DamageType damageType, AIStrategy aiStrategy, Guid? lootTableId)
    {
        return new Monster
        {
            Id = id,
            Name = name,
            Description = description,
            Level = level,
            HitPoints = hitPoints,
            ArmorClass = armorClass,
            Strength = str,
            Dexterity = dex,
            Constitution = con,
            Intelligence = intel,
            Wisdom = wis,
            Charisma = cha,
            ChallengeRating = challengeRating,
            ExperienceReward = experienceReward,
            AttackDice = attackDice,
            DamageType = damageType,
            AIStrategy = aiStrategy,
            LootTableId = lootTableId
        };
    }
}
