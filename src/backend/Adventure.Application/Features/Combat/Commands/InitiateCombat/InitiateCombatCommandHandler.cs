using Adventure.Domain.Combat;
using Adventure.Domain.Entities;
using Adventure.Domain.Enums;
using Adventure.Domain.Interfaces;
using Adventure.Domain.Rules;
using MediatR;

namespace Adventure.Application.Features.Combat.Commands.InitiateCombat;

public class InitiateCombatCommandHandler : IRequestHandler<InitiateCombatCommand, CombatStateDto>
{
    private readonly IRepository<Domain.Entities.Character> _characterRepo;
    private readonly IRepository<Monster> _monsterRepo;
    private readonly ICombatEncounterStore _encounterStore;
    private readonly IDiceService _dice;
    private readonly CombatTurnResolver _turnResolver;

    public InitiateCombatCommandHandler(
        IRepository<Domain.Entities.Character> characterRepo,
        IRepository<Monster> monsterRepo,
        ICombatEncounterStore encounterStore,
        IDiceService dice,
        CombatTurnResolver turnResolver)
    {
        _characterRepo = characterRepo;
        _monsterRepo = monsterRepo;
        _encounterStore = encounterStore;
        _dice = dice;
        _turnResolver = turnResolver;
    }

    public async Task<CombatStateDto> Handle(InitiateCombatCommand request, CancellationToken cancellationToken)
    {
        if (_encounterStore.HasActiveEncounter(request.CharacterId))
            throw new InvalidOperationException("Character already has an active combat encounter.");

        var character = await _characterRepo.GetByIdAsync(request.CharacterId, cancellationToken)
            ?? throw new KeyNotFoundException($"Character with id {request.CharacterId} not found.");

        if (character.CurrentHitPoints <= 0)
            throw new InvalidOperationException("Character is dead and cannot enter combat.");

        var encounter = CombatEncounter.Create(request.CharacterId);

        var playerParticipant = CombatParticipant.FromCharacter(character);
        var playerInitiative = _dice.RollD20() + AbilityModifierCalculator.Calculate(character.Dexterity);
        playerParticipant.SetInitiative(playerInitiative);
        encounter.AddParticipant(playerParticipant);

        var monsterNames = new List<string>();
        foreach (var monsterId in request.MonsterIds)
        {
            var monster = await _monsterRepo.GetByIdAsync(monsterId, cancellationToken)
                ?? throw new KeyNotFoundException($"Monster with id {monsterId} not found.");

            var instanceId = Guid.NewGuid();
            var monsterParticipant = CombatParticipant.FromMonster(monster, instanceId);
            var monsterInitiative = _dice.RollD20() + AbilityModifierCalculator.Calculate(monster.Dexterity);
            monsterParticipant.SetInitiative(monsterInitiative);
            encounter.AddParticipant(monsterParticipant);
            monsterNames.Add(monster.Name);
        }

        var enemyList = string.Join(" and ", monsterNames);
        encounter.AddLogEntry($"Combat begins! {enemyList} appear{(monsterNames.Count == 1 ? "s" : "")}!");

        foreach (var p in encounter.TurnOrder)
        {
            encounter.AddLogEntry($"{p.Name} rolls initiative: {p.InitiativeRoll}");
        }

        encounter.AddLogEntry($"{encounter.ActiveParticipant.Name} goes first!");

        _encounterStore.Add(request.CharacterId, encounter);

        // If enemies go first, auto-resolve their turns so the player isn't stuck
        if (encounter.ActiveParticipant.ParticipantType == ParticipantType.Enemy)
        {
            _turnResolver.ResolveMonsterTurns(encounter);
            await _turnResolver.PersistCombatResultIfComplete(encounter, cancellationToken);
        }

        return encounter.ToStateDto();
    }
}
