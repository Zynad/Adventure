using Adventure.Application.Features.Combat.Commands.EndTurn;
using Adventure.Application.Features.Combat.Commands.ExecuteCombatAction;
using Adventure.Domain.Enums;
using MediatR;

namespace Adventure.Application.Features.Combat.Commands.ExecuteAttack;

public class ExecuteAttackCommandHandler : IRequestHandler<ExecuteAttackCommand, CombatActionResultDto>
{
    private readonly IMediator _mediator;
    private readonly ICombatEncounterStore _encounterStore;

    public ExecuteAttackCommandHandler(IMediator mediator, ICombatEncounterStore encounterStore)
    {
        _mediator = mediator;
        _encounterStore = encounterStore;
    }

    public async Task<CombatActionResultDto> Handle(ExecuteAttackCommand request, CancellationToken cancellationToken)
    {
        // Delegate to the generalized action command
        var actionResult = await _mediator.Send(
            new ExecuteCombatActionCommand(request.CharacterId, CombatActionType.Attack, request.TargetId),
            cancellationToken);

        // Auto end turn for backward compatibility (old behavior resolved monster turns inline)
        var endTurnState = await _mediator.Send(
            new EndTurnCommand(request.CharacterId),
            cancellationToken);

        // Return action result with the post-monster-turns state
        return actionResult with { UpdatedState = endTurnState };
    }
}
