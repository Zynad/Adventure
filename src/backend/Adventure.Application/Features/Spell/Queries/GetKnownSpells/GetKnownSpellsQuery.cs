using MediatR;

namespace Adventure.Application.Features.Spell.Queries.GetKnownSpells;

public record GetKnownSpellsQuery(Guid CharacterId) : IRequest<CharacterSpellInfoDto>;
