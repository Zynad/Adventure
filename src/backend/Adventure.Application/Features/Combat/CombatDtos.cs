namespace Adventure.Application.Features.Combat;

public record CombatParticipantDto(
    Guid Id,
    string Name,
    int ParticipantType,
    int CurrentHp,
    int MaxHp,
    int ArmorClass,
    int InitiativeRoll,
    bool IsAlive,
    IReadOnlyList<int> ActiveConditions);

public record CombatStateDto(
    Guid EncounterId,
    int CurrentRound,
    Guid ActiveParticipantId,
    bool IsPlayerTurn,
    bool IsComplete,
    bool HasTakenAction,
    int? Outcome,
    IReadOnlyList<CombatParticipantDto> Participants,
    IReadOnlyList<string> CombatLog);

public record CombatActionResultDto(
    int ActionType,
    string ActorName,
    string? TargetName,
    int? NaturalRoll,
    int? TotalRoll,
    int? TargetAC,
    bool IsHit,
    bool IsCritical,
    int DamageDealt,
    int HealingDone,
    string Description,
    CombatStateDto UpdatedState);

public record CombatConsumableDto(
    Guid ItemId,
    string Name,
    string EffectType,
    int EffectValue,
    int Quantity);
