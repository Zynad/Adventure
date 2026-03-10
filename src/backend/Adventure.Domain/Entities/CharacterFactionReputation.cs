using Adventure.Domain.Enums;

namespace Adventure.Domain.Entities;

public class CharacterFactionReputation
{
    public Guid Id { get; private set; }
    public Guid CharacterId { get; private set; }
    public Guid FactionId { get; private set; }
    public int ReputationPoints { get; private set; }
    public FactionStanding Standing { get; private set; } = FactionStanding.Neutral;

    protected CharacterFactionReputation() { }
}
