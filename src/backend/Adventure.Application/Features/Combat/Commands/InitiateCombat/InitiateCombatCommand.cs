using MediatR;

namespace Adventure.Application.Features.Combat.Commands.InitiateCombat;

public record InitiateCombatCommand(Guid CharacterId, List<Guid> MonsterIds) : IRequest<CombatStateDto>;
