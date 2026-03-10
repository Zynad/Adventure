using Adventure.Domain.Enums;
using Adventure.Domain.Interfaces;

namespace Adventure.Infrastructure.AI;

public class PlaceholderCombatAI : ICombatAI
{
    public CombatActionType DecideAction(Guid participantId, IReadOnlyList<CombatParticipantInfo> participants)
    {
        return CombatActionType.Attack;
    }
}
