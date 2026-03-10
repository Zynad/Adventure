using Adventure.Domain.Enums;

namespace Adventure.Domain.Events;

public sealed class CombatEndedEvent : DomainEvent
{
    public Guid CharacterId { get; }
    public Guid EncounterId { get; }
    public CombatOutcome Outcome { get; }

    public CombatEndedEvent(Guid characterId, Guid encounterId, CombatOutcome outcome)
    {
        CharacterId = characterId;
        EncounterId = encounterId;
        Outcome = outcome;
    }
}
