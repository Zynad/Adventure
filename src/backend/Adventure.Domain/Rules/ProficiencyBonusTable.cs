namespace Adventure.Domain.Rules;

public static class ProficiencyBonusTable
{
    public static int GetBonus(int level) => level switch
    {
        >= 1 and <= 4 => 2,
        >= 5 and <= 8 => 3,
        >= 9 and <= 12 => 4,
        >= 13 and <= 16 => 5,
        >= 17 and <= 20 => 6,
        _ => throw new ArgumentOutOfRangeException(nameof(level), "Level must be between 1 and 20.")
    };
}
