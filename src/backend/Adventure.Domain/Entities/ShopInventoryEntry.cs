namespace Adventure.Domain.Entities;

public class ShopInventoryEntry
{
    public Guid Id { get; private set; }
    public Guid ShopId { get; private set; }
    public Guid ItemId { get; private set; }
    public int Stock { get; private set; } = -1;
    public int? PriceOverride { get; private set; }

    // Navigation properties
    public Shop Shop { get; private set; } = null!;
    public Item Item { get; private set; } = null!;

    protected ShopInventoryEntry() { }
}
