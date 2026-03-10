namespace Adventure.Domain.Entities;

public class Npc
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public Guid ZoneId { get; private set; }
    public int PositionX { get; private set; }
    public int PositionY { get; private set; }
    public string? DialogueTree { get; private set; }
    public bool IsQuestGiver { get; private set; }
    public bool IsMerchant { get; private set; }
    public Guid? FactionId { get; private set; }

    // Navigation properties
    public Zone Zone { get; private set; } = null!;
    public Faction? Faction { get; private set; }
    public Shop? Shop { get; private set; }

    protected Npc() { }

    internal static Npc CreateForSeed(
        Guid id, string name, string description, Guid zoneId,
        int positionX, int positionY, string? dialogueTree,
        bool isQuestGiver, bool isMerchant, Guid? factionId)
    {
        return new Npc
        {
            Id = id,
            Name = name,
            Description = description,
            ZoneId = zoneId,
            PositionX = positionX,
            PositionY = positionY,
            DialogueTree = dialogueTree,
            IsQuestGiver = isQuestGiver,
            IsMerchant = isMerchant,
            FactionId = factionId
        };
    }
}
