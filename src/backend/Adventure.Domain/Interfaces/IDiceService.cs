using Adventure.Domain.ValueObjects;

namespace Adventure.Domain.Interfaces;

public interface IDiceService
{
    int Roll(int count, int sides);
    int Roll(DiceRoll dice);
    int RollD20();
}
