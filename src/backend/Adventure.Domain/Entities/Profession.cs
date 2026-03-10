using Adventure.Domain.Enums;

namespace Adventure.Domain.Entities;

public class Profession
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public ProfessionType ProfessionType { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public int MaxLevel { get; private set; } = 300;

    protected Profession() { }
}
