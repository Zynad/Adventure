namespace Adventure.Domain.Entities;

public class CharacterProfession
{
    public Guid Id { get; private set; }
    public Guid CharacterId { get; private set; }
    public Guid ProfessionId { get; private set; }
    public int SkillLevel { get; private set; } = 1;

    protected CharacterProfession() { }
}
