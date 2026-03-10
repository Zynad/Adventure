using Adventure.Domain.Enums;

namespace Adventure.Domain.Entities;

public class Equipment : Item
{
    public EquipmentSlotType EquipmentSlot { get; private set; }
    public int? ArmorBonus { get; private set; }
    public string? DamageDice { get; private set; }
    public DamageType? DamageType { get; private set; }
    public int RequiredLevel { get; private set; }
    public CharacterClass? RequiredClass { get; private set; }
    public string? StatBonuses { get; private set; }

    protected Equipment() { }

    internal static Equipment CreateForSeed(
        Guid id, string name, string description, ItemRarity rarity,
        decimal weight, int baseValue, bool isStackable,
        EquipmentSlotType equipmentSlot, int? armorBonus, string? damageDice,
        DamageType? damageType, int requiredLevel, CharacterClass? requiredClass, string? statBonuses)
    {
        return new Equipment
        {
            Id = id,
            Name = name,
            Description = description,
            ItemType = ItemType.Equipment,
            Rarity = rarity,
            Weight = weight,
            BaseValue = baseValue,
            IsStackable = isStackable,
            EquipmentSlot = equipmentSlot,
            ArmorBonus = armorBonus,
            DamageDice = damageDice,
            DamageType = damageType,
            RequiredLevel = requiredLevel,
            RequiredClass = requiredClass,
            StatBonuses = statBonuses
        };
    }
}
