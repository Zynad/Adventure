namespace Adventure.Domain.Rules;

public static class ExperienceTable
{
    private static readonly Dictionary<int, int> XpThresholds = new()
    {
        { 1, 0 },
        { 2, 300 },
        { 3, 900 },
        { 4, 2700 },
        { 5, 6500 },
        { 6, 14000 },
        { 7, 23000 },
        { 8, 34000 },
        { 9, 48000 },
        { 10, 64000 },
        { 11, 85000 },
        { 12, 100000 },
        { 13, 120000 },
        { 14, 140000 },
        { 15, 165000 },
        { 16, 195000 },
        { 17, 225000 },
        { 18, 265000 },
        { 19, 305000 },
        { 20, 355000 }
    };

    /// <summary>
    /// Returns the total XP required to reach the specified level.
    /// </summary>
    public static int GetRequiredXp(int level)
    {
        if (level < 1 || level > 20)
            throw new ArgumentOutOfRangeException(nameof(level), "Level must be between 1 and 20.");

        return XpThresholds[level];
    }
}
