using MediatR;

namespace Adventure.Application.Features.Combat.Commands.ExecuteAttack;

public record ExecuteAttackCommand(Guid CharacterId, Guid TargetId) : IRequest<CombatActionResultDto>;
