namespace Adventure.Domain.Events;

public sealed class ItemAcquiredEvent : DomainEvent
{
    public Guid CharacterId { get; }
    public Guid ItemId { get; }
    public int Quantity { get; }

    public ItemAcquiredEvent(Guid characterId, Guid itemId, int quantity)
    {
        CharacterId = characterId;
        ItemId = itemId;
        Quantity = quantity;
    }
}
