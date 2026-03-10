namespace Adventure.Domain.Rules;

public static class PointBuyCostTable
{
    public const int TotalPoints = 27;
    public const int MinScore = 8;
    public const int MaxScore = 15;

    private static readonly Dictionary<int, int> Costs = new()
    {
        { 8, 0 }, { 9, 1 }, { 10, 2 }, { 11, 3 },
        { 12, 4 }, { 13, 5 }, { 14, 7 }, { 15, 9 }
    };

    public static int GetCost(int score) =>
        Costs.TryGetValue(score, out var cost)
            ? cost
            : throw new ArgumentOutOfRangeException(nameof(score), $"Score must be between {MinScore} and {MaxScore}.");

    public static int CalculateTotalCost(int str, int dex, int con, int intel, int wis, int cha) =>
        GetCost(str) + GetCost(dex) + GetCost(con) + GetCost(intel) + GetCost(wis) + GetCost(cha);

    public static bool IsValid(int str, int dex, int con, int intel, int wis, int cha) =>
        CalculateTotalCost(str, dex, con, intel, wis, cha) <= TotalPoints;
}
