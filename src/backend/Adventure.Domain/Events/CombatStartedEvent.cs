namespace Adventure.Domain.Events;

public sealed class CombatStartedEvent : DomainEvent
{
    public Guid CharacterId { get; }
    public Guid EncounterId { get; }
    public IReadOnlyList<Guid> MonsterIds { get; }

    public CombatStartedEvent(Guid characterId, Guid encounterId, IReadOnlyList<Guid> monsterIds)
    {
        CharacterId = characterId;
        EncounterId = encounterId;
        MonsterIds = monsterIds;
    }
}
