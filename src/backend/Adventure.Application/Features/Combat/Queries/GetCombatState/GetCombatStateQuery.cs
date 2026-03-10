using MediatR;

namespace Adventure.Application.Features.Combat.Queries.GetCombatState;

public record GetCombatStateQuery(Guid CharacterId) : IRequest<CombatStateDto>;
