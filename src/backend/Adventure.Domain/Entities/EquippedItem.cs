using Adventure.Domain.Enums;

namespace Adventure.Domain.Entities;

public class EquippedItem
{
    public Guid Id { get; private set; }
    public Guid CharacterId { get; private set; }
    public Guid ItemId { get; private set; }
    public EquipmentSlotType SlotType { get; private set; }

    // Navigation properties
    public Character Character { get; private set; } = null!;
    public Item Item { get; private set; } = null!;

    protected EquippedItem() { }
}
