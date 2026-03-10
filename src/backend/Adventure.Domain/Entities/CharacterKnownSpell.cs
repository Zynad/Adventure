namespace Adventure.Domain.Entities;

public class CharacterKnownSpell
{
    public Guid CharacterId { get; private set; }
    public Guid SpellId { get; private set; }

    public Character Character { get; private set; } = null!;
    public Spell Spell { get; private set; } = null!;

    protected CharacterKnownSpell() { }

    public static CharacterKnownSpell Create(Guid characterId, Guid spellId)
    {
        return new CharacterKnownSpell
        {
            CharacterId = characterId,
            SpellId = spellId
        };
    }

    internal static CharacterKnownSpell CreateForSeed(Guid characterId, Guid spellId)
    {
        return new CharacterKnownSpell
        {
            CharacterId = characterId,
            SpellId = spellId
        };
    }
}
