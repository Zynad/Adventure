namespace Adventure.Domain.Entities;

public class LootTable
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;

    // Navigation properties
    public ICollection<LootTableEntry> Entries { get; private set; } = new List<LootTableEntry>();

    protected LootTable() { }
}
