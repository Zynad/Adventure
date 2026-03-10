namespace Adventure.Domain.Entities;

public class LootTableEntry
{
    public Guid Id { get; private set; }
    public Guid LootTableId { get; private set; }
    public Guid ItemId { get; private set; }
    public decimal DropChance { get; private set; }
    public int MinQuantity { get; private set; } = 1;
    public int MaxQuantity { get; private set; } = 1;

    protected LootTableEntry() { }
}
