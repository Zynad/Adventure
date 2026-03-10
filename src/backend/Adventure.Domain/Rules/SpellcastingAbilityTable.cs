using Adventure.Domain.Enums;

namespace Adventure.Domain.Rules;

public static class SpellcastingAbilityTable
{
    public static AbilityType? GetSpellcastingAbility(CharacterClass characterClass)
    {
        return characterClass switch
        {
            CharacterClass.Wizard => AbilityType.Intelligence,
            CharacterClass.Cleric => AbilityType.Wisdom,
            CharacterClass.Paladin => AbilityType.Charisma,
            CharacterClass.Ranger => AbilityType.Wisdom,
            _ => null
        };
    }
}
