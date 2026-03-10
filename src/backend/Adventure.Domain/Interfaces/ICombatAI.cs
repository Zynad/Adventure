using Adventure.Domain.Enums;

namespace Adventure.Domain.Interfaces;

public record CombatParticipantInfo(
    Guid Id,
    string Name,
    ParticipantType Type,
    int CurrentHp,
    int MaxHp,
    int ArmorClass,
    AIStrategy Strategy);

public interface ICombatAI
{
    CombatActionType DecideAction(Guid participantId, IReadOnlyList<CombatParticipantInfo> participants);
}
