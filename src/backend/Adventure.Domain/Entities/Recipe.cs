namespace Adventure.Domain.Entities;

public class Recipe
{
    public Guid Id { get; private set; }
    public Guid ProfessionId { get; private set; }
    public Guid OutputItemId { get; private set; }
    public int OutputQuantity { get; private set; } = 1;
    public int RequiredSkillLevel { get; private set; }
    public decimal SkillGainChance { get; private set; } = 1.0m;
    public int CraftingTimeSeconds { get; private set; } = 5;

    // Navigation properties
    public ICollection<RecipeIngredient> Ingredients { get; private set; } = new List<RecipeIngredient>();

    protected Recipe() { }
}
