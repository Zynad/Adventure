using MediatR;

namespace Adventure.Application.Features.Spell.Commands.LearnSpell;

public record LearnSpellCommand(Guid CharacterId, Guid SpellId) : IRequest<KnownSpellDto>;
