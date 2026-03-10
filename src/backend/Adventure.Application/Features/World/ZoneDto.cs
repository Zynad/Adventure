namespace Adventure.Application.Features.World;

public record ZoneDto(
    Guid Id,
    string Name,
    string Description,
    int ZoneType,
    int Width,
    int Height,
    int WorldMapX,
    int WorldMapY,
    bool IsDiscovered);
