using Adventure.Domain.Enums;

namespace Adventure.Domain.ValueObjects;

public record AbilityScores(
    int Strength,
    int Dexterity,
    int Constitution,
    int Intelligence,
    int Wisdom,
    int Charisma)
{
    public int GetScore(AbilityType ability) => ability switch
    {
        AbilityType.Strength => Strength,
        AbilityType.Dexterity => Dexterity,
        AbilityType.Constitution => Constitution,
        AbilityType.Intelligence => Intelligence,
        AbilityType.Wisdom => Wisdom,
        AbilityType.Charisma => Charisma,
        _ => throw new ArgumentOutOfRangeException(nameof(ability), ability, "Unknown ability type.")
    };

    public int GetModifier(AbilityType ability) =>
        (int)Math.Floor((GetScore(ability) - 10) / 2.0);
}
