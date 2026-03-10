namespace Adventure.Domain.Entities;

public class Shop
{
    public Guid Id { get; private set; }
    public Guid NpcId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public decimal BuyPriceModifier { get; private set; } = 1.0m;
    public decimal SellPriceModifier { get; private set; } = 0.5m;

    // Navigation properties
    public Npc Npc { get; private set; } = null!;
    public ICollection<ShopInventoryEntry> Inventory { get; private set; } = new List<ShopInventoryEntry>();

    protected Shop() { }
}
