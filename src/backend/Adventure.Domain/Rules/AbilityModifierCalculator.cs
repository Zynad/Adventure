namespace Adventure.Domain.Rules;

public static class AbilityModifierCalculator
{
    public static int Calculate(int score) => (int)Math.Floor((score - 10) / 2.0);
}
