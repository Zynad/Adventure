namespace Adventure.Application.Features.Character;

public static class CharacterMappingExtensions
{
    public static CharacterDto ToDto(this Domain.Entities.Character character)
    {
        return new CharacterDto(
            character.Id,
            character.Name,
            (int)character.Race,
            (int)character.Class,
            character.Level,
            character.ExperiencePoints,
            character.CurrentHitPoints,
            character.MaxHitPoints,
            character.ArmorClass,
            character.Strength,
            character.Dexterity,
            character.Constitution,
            character.Intelligence,
            character.Wisdom,
            character.Charisma,
            character.Gold,
            character.CurrentZoneId,
            character.PositionX,
            character.PositionY);
    }
}
