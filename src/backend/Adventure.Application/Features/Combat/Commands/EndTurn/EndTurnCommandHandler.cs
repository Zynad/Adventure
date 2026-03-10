using Adventure.Domain.Combat;
using Adventure.Domain.Enums;
using MediatR;

namespace Adventure.Application.Features.Combat.Commands.EndTurn;

public class EndTurnCommandHandler : IRequestHandler<EndTurnCommand, CombatStateDto>
{
    private readonly ICombatEncounterStore _encounterStore;
    private readonly CombatTurnResolver _turnResolver;

    public EndTurnCommandHandler(
        ICombatEncounterStore encounterStore,
        CombatTurnResolver turnResolver)
    {
        _encounterStore = encounterStore;
        _turnResolver = turnResolver;
    }

    public async Task<CombatStateDto> Handle(EndTurnCommand request, CancellationToken cancellationToken)
    {
        var encounter = _encounterStore.GetByCharacterId(request.CharacterId)
            ?? throw new KeyNotFoundException("No active combat encounter found.");

        if (encounter.IsComplete)
            throw new InvalidOperationException("Combat has already ended.");

        var active = encounter.ActiveParticipant;
        if (active.ParticipantType != ParticipantType.Player || active.Id != request.CharacterId)
            throw new InvalidOperationException("It is not the player's turn.");

        if (!encounter.HasTakenAction)
            throw new InvalidOperationException("You must take an action before ending your turn.");

        encounter.AdvanceTurn();

        _turnResolver.ResolveMonsterTurns(encounter);
        await _turnResolver.PersistCombatResultIfComplete(encounter, cancellationToken);

        return encounter.ToStateDto();
    }
}
