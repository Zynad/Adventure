using Adventure.Domain.Combat;
using Adventure.Domain.Enums;

namespace Adventure.Application.Features.Combat;

public static class CombatMappingExtensions
{
    public static CombatParticipantDto ToDto(this CombatParticipant participant)
    {
        return new CombatParticipantDto(
            participant.Id,
            participant.Name,
            (int)participant.ParticipantType,
            participant.CurrentHp,
            participant.MaxHp,
            participant.ArmorClass,
            participant.InitiativeRoll,
            participant.IsAlive,
            participant.ActiveConditions.Select(c => (int)c).ToList());
    }

    public static CombatStateDto ToStateDto(this CombatEncounter encounter)
    {
        var active = encounter.ActiveParticipant;
        return new CombatStateDto(
            encounter.Id,
            encounter.CurrentRound,
            active.Id,
            active.ParticipantType == ParticipantType.Player,
            encounter.IsComplete,
            encounter.HasTakenAction,
            encounter.Outcome.HasValue ? (int)encounter.Outcome.Value : null,
            encounter.TurnOrder.Select(p => p.ToDto()).ToList(),
            encounter.CombatLog.ToList());
    }
}
