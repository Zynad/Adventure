namespace Adventure.Domain.Entities;

public class Faction
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string? IconSprite { get; private set; }

    protected Faction() { }
}
