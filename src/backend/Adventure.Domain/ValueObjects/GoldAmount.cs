namespace Adventure.Domain.ValueObjects;

public record GoldAmount : IComparable<GoldAmount>
{
    public int Value { get; }

    public GoldAmount(int value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value), "Gold amount cannot be negative.");

        Value = value;
    }

    public static GoldAmount Zero => new(0);

    public static GoldAmount operator +(GoldAmount left, GoldAmount right) =>
        new(left.Value + right.Value);

    public static GoldAmount operator -(GoldAmount left, GoldAmount right)
    {
        var result = left.Value - right.Value;
        if (result < 0)
            throw new InvalidOperationException("Gold amount cannot be negative.");
        return new GoldAmount(result);
    }

    public static bool operator >(GoldAmount left, GoldAmount right) =>
        left.Value > right.Value;

    public static bool operator <(GoldAmount left, GoldAmount right) =>
        left.Value < right.Value;

    public static bool operator >=(GoldAmount left, GoldAmount right) =>
        left.Value >= right.Value;

    public static bool operator <=(GoldAmount left, GoldAmount right) =>
        left.Value <= right.Value;

    public int CompareTo(GoldAmount? other) =>
        other is null ? 1 : Value.CompareTo(other.Value);

    public override string ToString() => $"{Value} gp";
}
