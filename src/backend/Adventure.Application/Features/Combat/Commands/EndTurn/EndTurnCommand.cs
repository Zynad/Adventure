using MediatR;

namespace Adventure.Application.Features.Combat.Commands.EndTurn;

public record EndTurnCommand(Guid CharacterId) : IRequest<CombatStateDto>;
