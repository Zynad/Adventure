namespace Adventure.Domain.Events;

public sealed class ZoneEnteredEvent : DomainEvent
{
    public Guid CharacterId { get; }
    public Guid ZoneId { get; }
    public Guid? PreviousZoneId { get; }

    public ZoneEnteredEvent(Guid characterId, Guid zoneId, Guid? previousZoneId)
    {
        CharacterId = characterId;
        ZoneId = zoneId;
        PreviousZoneId = previousZoneId;
    }
}
