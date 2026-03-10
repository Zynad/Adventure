namespace Adventure.Domain.Entities;

public class RecipeIngredient
{
    public Guid Id { get; private set; }
    public Guid RecipeId { get; private set; }
    public Guid ItemId { get; private set; }
    public int Quantity { get; private set; }

    protected RecipeIngredient() { }
}
