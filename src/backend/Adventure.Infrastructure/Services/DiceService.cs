using Adventure.Domain.Interfaces;
using Adventure.Domain.ValueObjects;

namespace Adventure.Infrastructure.Services;

public class DiceService : IDiceService
{
    private readonly Random _random = new();

    public int Roll(int count, int sides)
    {
        var total = 0;
        for (var i = 0; i < count; i++)
        {
            total += _random.Next(1, sides + 1);
        }
        return total;
    }

    public int Roll(DiceRoll dice)
    {
        return Roll(dice.Count, dice.Sides) + dice.Modifier;
    }

    public int RollD20()
    {
        return Roll(1, 20);
    }
}
