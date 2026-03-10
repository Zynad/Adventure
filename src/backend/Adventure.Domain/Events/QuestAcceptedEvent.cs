namespace Adventure.Domain.Events;

public sealed class QuestAcceptedEvent : DomainEvent
{
    public Guid CharacterId { get; }
    public Guid QuestId { get; }

    public QuestAcceptedEvent(Guid characterId, Guid questId)
    {
        CharacterId = characterId;
        QuestId = questId;
    }
}
