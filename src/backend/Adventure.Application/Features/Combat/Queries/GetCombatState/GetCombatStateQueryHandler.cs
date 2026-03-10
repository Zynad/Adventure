using MediatR;

namespace Adventure.Application.Features.Combat.Queries.GetCombatState;

public class GetCombatStateQueryHandler : IRequestHandler<GetCombatStateQuery, CombatStateDto>
{
    private readonly ICombatEncounterStore _encounterStore;

    public GetCombatStateQueryHandler(ICombatEncounterStore encounterStore)
    {
        _encounterStore = encounterStore;
    }

    public Task<CombatStateDto> Handle(GetCombatStateQuery request, CancellationToken cancellationToken)
    {
        var encounter = _encounterStore.GetByCharacterId(request.CharacterId)
            ?? throw new KeyNotFoundException("No active combat encounter found.");

        return Task.FromResult(encounter.ToStateDto());
    }
}
