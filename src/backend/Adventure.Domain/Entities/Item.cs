using Adventure.Domain.Enums;

namespace Adventure.Domain.Entities;

public class Item
{
    public Guid Id { get; private protected set; }
    public string Name { get; private protected set; } = string.Empty;
    public string Description { get; private protected set; } = string.Empty;
    public ItemType ItemType { get; private protected set; }
    public ItemRarity Rarity { get; private protected set; }
    public decimal Weight { get; private protected set; }
    public int BaseValue { get; private protected set; }
    public bool IsStackable { get; private protected set; }

    protected Item() { }
}
