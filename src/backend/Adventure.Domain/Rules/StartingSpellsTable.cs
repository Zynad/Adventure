using Adventure.Domain.Enums;

namespace Adventure.Domain.Rules;

public static class StartingSpellsTable
{
    public static IReadOnlyList<string> GetStartingSpellNames(CharacterClass characterClass)
    {
        return characterClass switch
        {
            CharacterClass.Wizard => ["Fire Bolt", "Magic Missile", "Ray of Frost"],
            CharacterClass.Cleric => ["Sacred Flame", "Cure Wounds", "Healing Word"],
            _ => []
        };
    }
}
