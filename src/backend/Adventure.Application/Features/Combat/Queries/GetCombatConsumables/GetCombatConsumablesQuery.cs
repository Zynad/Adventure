using MediatR;

namespace Adventure.Application.Features.Combat.Queries.GetCombatConsumables;

public record GetCombatConsumablesQuery(Guid CharacterId) : IRequest<IReadOnlyList<CombatConsumableDto>>;
