namespace Adventure.Domain.Events;

public sealed class CharacterDiedEvent : DomainEvent
{
    public Guid CharacterId { get; }

    public CharacterDiedEvent(Guid characterId)
    {
        CharacterId = characterId;
    }
}
