namespace Adventure.Application.Features.Character;

public record CharacterDto(
    Guid Id,
    string Name,
    int Race,
    int CharacterClass,
    int Level,
    int ExperiencePoints,
    int CurrentHitPoints,
    int MaxHitPoints,
    int ArmorClass,
    int Strength,
    int Dexterity,
    int Constitution,
    int Intelligence,
    int Wisdom,
    int Charisma,
    int Gold,
    Guid CurrentZoneId,
    int PositionX,
    int PositionY);
