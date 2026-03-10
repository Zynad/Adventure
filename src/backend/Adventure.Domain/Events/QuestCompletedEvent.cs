namespace Adventure.Domain.Events;

public sealed class QuestCompletedEvent : DomainEvent
{
    public Guid CharacterId { get; }
    public Guid QuestId { get; }

    public QuestCompletedEvent(Guid characterId, Guid questId)
    {
        CharacterId = characterId;
        QuestId = questId;
    }
}
