using MediatR;

namespace Adventure.Application.Features.Combat.Commands.UseItemInCombat;

public record UseItemInCombatCommand(Guid CharacterId, Guid ItemId) : IRequest<CombatActionResultDto>;
