using Adventure.Domain.Enums;

namespace Adventure.Domain.Entities;

public class QuestObjective
{
    public Guid Id { get; private set; }
    public Guid QuestId { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public QuestObjectiveType ObjectiveType { get; private set; }
    public Guid? TargetId { get; private set; }
    public int RequiredCount { get; private set; }
    public int OrderIndex { get; private set; }

    protected QuestObjective() { }
}
