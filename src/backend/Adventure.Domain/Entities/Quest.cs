namespace Adventure.Domain.Entities;

public class Quest
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public Guid QuestGiverNpcId { get; private set; }
    public Guid? FactionId { get; private set; }
    public int RequiredLevel { get; private set; }
    public int RewardExperience { get; private set; }
    public int RewardGold { get; private set; }
    public Guid? RewardItemId { get; private set; }
    public int ReputationReward { get; private set; }
    public bool IsRepeatable { get; private set; }

    // Navigation properties
    public ICollection<QuestObjective> Objectives { get; private set; } = new List<QuestObjective>();

    protected Quest() { }
}
