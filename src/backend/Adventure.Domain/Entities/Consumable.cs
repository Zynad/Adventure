using Adventure.Domain.Enums;

namespace Adventure.Domain.Entities;

public class Consumable : Item
{
    public string EffectType { get; private set; } = string.Empty;
    public int EffectValue { get; private set; }

    protected Consumable() { }

    internal static Consumable CreateForSeed(
        Guid id, string name, string description, ItemRarity rarity,
        decimal weight, int baseValue, bool isStackable,
        string effectType, int effectValue)
    {
        return new Consumable
        {
            Id = id,
            Name = name,
            Description = description,
            ItemType = ItemType.Consumable,
            Rarity = rarity,
            Weight = weight,
            BaseValue = baseValue,
            IsStackable = isStackable,
            EffectType = effectType,
            EffectValue = effectValue
        };
    }
}
