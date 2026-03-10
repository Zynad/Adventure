namespace Adventure.Domain.Entities;

public class ZoneConnection
{
    public Guid Id { get; private set; }
    public Guid FromZoneId { get; private set; }
    public Guid ToZoneId { get; private set; }
    public int FromX { get; private set; }
    public int FromY { get; private set; }
    public int ToX { get; private set; }
    public int ToY { get; private set; }

    protected ZoneConnection() { }

    internal static ZoneConnection CreateForSeed(
        Guid id, Guid fromZoneId, Guid toZoneId,
        int fromX, int fromY, int toX, int toY)
    {
        return new ZoneConnection
        {
            Id = id,
            FromZoneId = fromZoneId,
            ToZoneId = toZoneId,
            FromX = fromX,
            FromY = fromY,
            ToX = toX,
            ToY = toY
        };
    }
}
