using Adventure.Domain.Entities;

namespace Adventure.Application.Features.World;

public static class WorldMappingExtensions
{
    public static ZoneDto ToDto(this Zone zone)
    {
        return new ZoneDto(
            zone.Id,
            zone.Name,
            zone.Description,
            (int)zone.ZoneType,
            zone.Width,
            zone.Height,
            zone.WorldMapX,
            zone.WorldMapY,
            zone.IsDiscovered);
    }

    public static TileDto ToDto(this Tile tile)
    {
        return new TileDto(
            tile.Id,
            tile.ZoneId,
            tile.X,
            tile.Y,
            (int)tile.TileType,
            tile.IsWalkable,
            tile.IsInteractable,
            tile.InteractionData,
            tile.TilesetSpriteName);
    }
}
