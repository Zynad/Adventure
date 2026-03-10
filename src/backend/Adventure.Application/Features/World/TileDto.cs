namespace Adventure.Application.Features.World;

public record TileDto(
    Guid Id,
    Guid ZoneId,
    int X,
    int Y,
    int TileType,
    bool IsWalkable,
    bool IsInteractable,
    string? InteractionData,
    string TilesetSpriteName);
