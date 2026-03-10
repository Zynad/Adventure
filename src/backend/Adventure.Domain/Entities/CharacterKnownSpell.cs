namespace Adventure.Domain.Entities;

public class CharacterKnownSpell
{
    public Guid CharacterId { get; private set; }
    public Guid SpellId { get; private set; }

    protected CharacterKnownSpell() { }
}
