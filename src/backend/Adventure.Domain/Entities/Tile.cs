using Adventure.Domain.Enums;

namespace Adventure.Domain.Entities;

public class Tile
{
    public Guid Id { get; private set; }
    public Guid ZoneId { get; private set; }
    public int X { get; private set; }
    public int Y { get; private set; }
    public TileType TileType { get; private set; }
    public bool IsWalkable { get; private set; }
    public bool IsInteractable { get; private set; }
    public string? InteractionData { get; private set; }
    public string TilesetSpriteName { get; private set; } = string.Empty;

    // Navigation properties
    public Zone Zone { get; private set; } = null!;

    protected Tile() { }

    internal static Tile CreateForSeed(
        Guid id, Guid zoneId, int x, int y, TileType tileType,
        bool isWalkable, bool isInteractable, string? interactionData, string tilesetSpriteName)
    {
        return new Tile
        {
            Id = id,
            ZoneId = zoneId,
            X = x,
            Y = y,
            TileType = tileType,
            IsWalkable = isWalkable,
            IsInteractable = isInteractable,
            InteractionData = interactionData,
            TilesetSpriteName = tilesetSpriteName
        };
    }
}
