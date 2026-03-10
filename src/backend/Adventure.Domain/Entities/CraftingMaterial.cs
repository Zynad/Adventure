namespace Adventure.Domain.Entities;

public class CraftingMaterial : Item
{
    public string MaterialCategory { get; private set; } = string.Empty;

    protected CraftingMaterial() { }
}
