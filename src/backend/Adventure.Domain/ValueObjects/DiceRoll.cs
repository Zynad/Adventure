namespace Adventure.Domain.ValueObjects;

public record DiceRoll(int Count, int Sides, int Modifier = 0)
{
    public override string ToString()
    {
        var result = $"{Count}d{Sides}";
        if (Modifier > 0)
            result += $"+{Modifier}";
        else if (Modifier < 0)
            result += Modifier.ToString();
        return result;
    }

    public static DiceRoll Parse(string notation)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(notation);

        var span = notation.AsSpan().Trim();
        var dIndex = span.IndexOf('d');

        if (dIndex < 0)
            throw new FormatException($"Invalid dice notation: '{notation}'. Expected format like '2d6+3'.");

        var count = int.Parse(span[..dIndex]);

        var remaining = span[(dIndex + 1)..];
        var modifier = 0;

        var plusIndex = remaining.IndexOf('+');
        var minusIndex = remaining.IndexOf('-');

        int modifierSeparator;
        if (plusIndex >= 0 && minusIndex >= 0)
            modifierSeparator = Math.Min(plusIndex, minusIndex);
        else if (plusIndex >= 0)
            modifierSeparator = plusIndex;
        else if (minusIndex >= 0)
            modifierSeparator = minusIndex;
        else
            modifierSeparator = -1;

        int sides;
        if (modifierSeparator >= 0)
        {
            sides = int.Parse(remaining[..modifierSeparator]);
            modifier = int.Parse(remaining[modifierSeparator..]);
        }
        else
        {
            sides = int.Parse(remaining);
        }

        return new DiceRoll(count, sides, modifier);
    }
}
