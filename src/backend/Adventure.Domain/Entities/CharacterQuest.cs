using Adventure.Domain.Enums;

namespace Adventure.Domain.Entities;

public class CharacterQuest
{
    public Guid Id { get; private set; }
    public Guid CharacterId { get; private set; }
    public Guid QuestId { get; private set; }
    public QuestStatus Status { get; private set; }
    public int CurrentObjectiveIndex { get; private set; }
    public string? Progress { get; private set; }
    public DateTime AcceptedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    protected CharacterQuest() { }
}
