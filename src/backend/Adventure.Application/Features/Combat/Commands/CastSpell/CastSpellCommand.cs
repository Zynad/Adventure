using MediatR;

namespace Adventure.Application.Features.Combat.Commands.CastSpell;

public record CastSpellCommand(
    Guid CharacterId,
    Guid SpellId,
    Guid? TargetId) : IRequest<CombatActionResultDto>;
