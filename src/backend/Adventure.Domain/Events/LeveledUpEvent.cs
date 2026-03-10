namespace Adventure.Domain.Events;

public sealed class LeveledUpEvent : DomainEvent
{
    public Guid CharacterId { get; }
    public int OldLevel { get; }
    public int NewLevel { get; }

    public LeveledUpEvent(Guid characterId, int oldLevel, int newLevel)
    {
        CharacterId = characterId;
        OldLevel = oldLevel;
        NewLevel = newLevel;
    }
}
