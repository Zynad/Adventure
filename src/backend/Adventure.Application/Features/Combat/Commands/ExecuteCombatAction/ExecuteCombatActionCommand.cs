using Adventure.Domain.Enums;
using MediatR;

namespace Adventure.Application.Features.Combat.Commands.ExecuteCombatAction;

public record ExecuteCombatActionCommand(
    Guid CharacterId,
    CombatActionType ActionType,
    Guid? TargetId) : IRequest<CombatActionResultDto>;
