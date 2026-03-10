namespace Adventure.Domain.Entities;

public class InventorySlot
{
    public Guid Id { get; private set; }
    public Guid CharacterId { get; private set; }
    public Guid ItemId { get; private set; }
    public int Quantity { get; private set; } = 1;
    public int SlotIndex { get; private set; }

    public bool IsEmpty => Quantity <= 0;

    // Navigation properties
    public Character Character { get; private set; } = null!;
    public Item Item { get; private set; } = null!;

    protected InventorySlot() { }

    public void UseOne()
    {
        Quantity = Math.Max(0, Quantity - 1);
    }
}
