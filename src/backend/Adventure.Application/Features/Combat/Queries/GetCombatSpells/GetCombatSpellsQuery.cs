using MediatR;

namespace Adventure.Application.Features.Combat.Queries.GetCombatSpells;

public record GetCombatSpellsQuery(Guid CharacterId) : IRequest<CombatSpellInfoDto>;
