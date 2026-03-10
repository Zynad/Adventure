using Adventure.Domain.Enums;

namespace Adventure.Domain.Rules;

public static class SpellSlotTable
{
    // D&D 5e full caster spell slot progression (Wizard, Cleric)
    private static readonly Dictionary<int, Dictionary<int, int>> FullCasterSlots = new()
    {
        [1]  = new() { [1] = 2 },
        [2]  = new() { [1] = 3 },
        [3]  = new() { [1] = 4, [2] = 2 },
        [4]  = new() { [1] = 4, [2] = 3 },
        [5]  = new() { [1] = 4, [2] = 3, [3] = 2 },
        [6]  = new() { [1] = 4, [2] = 3, [3] = 3 },
        [7]  = new() { [1] = 4, [2] = 3, [3] = 3, [4] = 1 },
        [8]  = new() { [1] = 4, [2] = 3, [3] = 3, [4] = 2 },
        [9]  = new() { [1] = 4, [2] = 3, [3] = 3, [4] = 3, [5] = 1 },
        [10] = new() { [1] = 4, [2] = 3, [3] = 3, [4] = 3, [5] = 2 },
        [11] = new() { [1] = 4, [2] = 3, [3] = 3, [4] = 3, [5] = 2, [6] = 1 },
        [12] = new() { [1] = 4, [2] = 3, [3] = 3, [4] = 3, [5] = 2, [6] = 1 },
        [13] = new() { [1] = 4, [2] = 3, [3] = 3, [4] = 3, [5] = 2, [6] = 1, [7] = 1 },
        [14] = new() { [1] = 4, [2] = 3, [3] = 3, [4] = 3, [5] = 2, [6] = 1, [7] = 1 },
        [15] = new() { [1] = 4, [2] = 3, [3] = 3, [4] = 3, [5] = 2, [6] = 1, [7] = 1, [8] = 1 },
        [16] = new() { [1] = 4, [2] = 3, [3] = 3, [4] = 3, [5] = 2, [6] = 1, [7] = 1, [8] = 1 },
        [17] = new() { [1] = 4, [2] = 3, [3] = 3, [4] = 3, [5] = 2, [6] = 1, [7] = 1, [8] = 1, [9] = 1 },
        [18] = new() { [1] = 4, [2] = 3, [3] = 3, [4] = 3, [5] = 3, [6] = 1, [7] = 1, [8] = 1, [9] = 1 },
        [19] = new() { [1] = 4, [2] = 3, [3] = 3, [4] = 3, [5] = 3, [6] = 2, [7] = 1, [8] = 1, [9] = 1 },
        [20] = new() { [1] = 4, [2] = 3, [3] = 3, [4] = 3, [5] = 3, [6] = 2, [7] = 2, [8] = 1, [9] = 1 },
    };

    // D&D 5e half caster spell slot progression (Paladin, Ranger) — starts at level 2
    private static readonly Dictionary<int, Dictionary<int, int>> HalfCasterSlots = new()
    {
        [1]  = new(),
        [2]  = new() { [1] = 2 },
        [3]  = new() { [1] = 3 },
        [4]  = new() { [1] = 3 },
        [5]  = new() { [1] = 4, [2] = 2 },
        [6]  = new() { [1] = 4, [2] = 2 },
        [7]  = new() { [1] = 4, [2] = 3 },
        [8]  = new() { [1] = 4, [2] = 3 },
        [9]  = new() { [1] = 4, [2] = 3, [3] = 2 },
        [10] = new() { [1] = 4, [2] = 3, [3] = 2 },
        [11] = new() { [1] = 4, [2] = 3, [3] = 3 },
        [12] = new() { [1] = 4, [2] = 3, [3] = 3 },
        [13] = new() { [1] = 4, [2] = 3, [3] = 3, [4] = 1 },
        [14] = new() { [1] = 4, [2] = 3, [3] = 3, [4] = 1 },
        [15] = new() { [1] = 4, [2] = 3, [3] = 3, [4] = 2 },
        [16] = new() { [1] = 4, [2] = 3, [3] = 3, [4] = 2 },
        [17] = new() { [1] = 4, [2] = 3, [3] = 3, [4] = 3, [5] = 1 },
        [18] = new() { [1] = 4, [2] = 3, [3] = 3, [4] = 3, [5] = 1 },
        [19] = new() { [1] = 4, [2] = 3, [3] = 3, [4] = 3, [5] = 2 },
        [20] = new() { [1] = 4, [2] = 3, [3] = 3, [4] = 3, [5] = 2 },
    };

    public static Dictionary<int, int> GetMaxSlots(CharacterClass characterClass, int characterLevel)
    {
        var level = Math.Clamp(characterLevel, 1, 20);

        return characterClass switch
        {
            CharacterClass.Wizard or CharacterClass.Cleric => new(FullCasterSlots[level]),
            CharacterClass.Paladin or CharacterClass.Ranger => new(HalfCasterSlots[level]),
            _ => new()
        };
    }

    public static bool IsSpellcaster(CharacterClass characterClass)
    {
        return characterClass is CharacterClass.Wizard or CharacterClass.Cleric
            or CharacterClass.Paladin or CharacterClass.Ranger;
    }
}
