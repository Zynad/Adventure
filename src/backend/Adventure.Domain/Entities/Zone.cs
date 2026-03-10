using Adventure.Domain.Enums;

namespace Adventure.Domain.Entities;

public class Zone
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public ZoneType ZoneType { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }
    public int WorldMapX { get; private set; }
    public int WorldMapY { get; private set; }
    public bool IsDiscovered { get; private set; }

    // Navigation properties
    public ICollection<Tile> Tiles { get; private set; } = new List<Tile>();
    public ICollection<ZoneConnection> Connections { get; private set; } = new List<ZoneConnection>();

    protected Zone() { }

    internal static Zone CreateForSeed(
        Guid id, string name, string description, ZoneType zoneType,
        int width, int height, int worldMapX, int worldMapY, bool isDiscovered)
    {
        return new Zone
        {
            Id = id,
            Name = name,
            Description = description,
            ZoneType = zoneType,
            Width = width,
            Height = height,
            WorldMapX = worldMapX,
            WorldMapY = worldMapY,
            IsDiscovered = isDiscovered
        };
    }
}
