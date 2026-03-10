using Adventure.Application.Features.Character;
using Adventure.Application.Features.Combat;
using Adventure.Application.Features.Combat.Commands.InitiateCombat;
using Adventure.Application.Features.Game;
using Adventure.Application.Features.World;
using Adventure.Domain.Combat;
using Adventure.Domain.Entities;
using Adventure.Domain.Enums;
using Adventure.Domain.Interfaces;
using MediatR;

namespace Adventure.Application.Features.Movement.Commands.MoveCharacter;

public class MoveCharacterCommandHandler : IRequestHandler<MoveCharacterCommand, MoveResultDto>
{
    private readonly IRepository<Domain.Entities.Character> _characterRepo;
    private readonly IRepository<Zone> _zoneRepo;
    private readonly IRepository<Tile> _tileRepo;
    private readonly IRepository<Npc> _npcRepo;
    private readonly IRepository<ZoneConnection> _connectionRepo;
    private readonly IRepository<Monster> _monsterRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICombatEncounterStore _encounterStore;
    private readonly IDiceService _dice;
    private readonly IMediator _mediator;

    public MoveCharacterCommandHandler(
        IRepository<Domain.Entities.Character> characterRepo,
        IRepository<Zone> zoneRepo,
        IRepository<Tile> tileRepo,
        IRepository<Npc> npcRepo,
        IRepository<ZoneConnection> connectionRepo,
        IRepository<Monster> monsterRepo,
        IUnitOfWork unitOfWork,
        ICombatEncounterStore encounterStore,
        IDiceService dice,
        IMediator mediator)
    {
        _characterRepo = characterRepo;
        _zoneRepo = zoneRepo;
        _tileRepo = tileRepo;
        _npcRepo = npcRepo;
        _connectionRepo = connectionRepo;
        _monsterRepo = monsterRepo;
        _unitOfWork = unitOfWork;
        _encounterStore = encounterStore;
        _dice = dice;
        _mediator = mediator;
    }

    public async Task<MoveResultDto> Handle(MoveCharacterCommand request, CancellationToken cancellationToken)
    {
        var character = await _characterRepo.GetByIdAsync(request.CharacterId, cancellationToken)
            ?? throw new KeyNotFoundException($"Character with id {request.CharacterId} not found.");

        var zone = await _zoneRepo.GetByIdAsync(character.CurrentZoneId, cancellationToken)
            ?? throw new KeyNotFoundException($"Zone with id {character.CurrentZoneId} not found.");

        var newX = character.PositionX + request.Dx;
        var newY = character.PositionY + request.Dy;

        // Bounds check
        if (newX < 0 || newX >= zone.Width || newY < 0 || newY >= zone.Height)
            return new MoveResultDto(false, "You can't go that way.", await BuildGameState(character, zone, cancellationToken));

        // Walkability check
        var tiles = await _tileRepo.FindAsync(t => t.ZoneId == zone.Id && t.X == newX && t.Y == newY, cancellationToken);
        var tile = tiles.FirstOrDefault();

        if (tile is null || !tile.IsWalkable)
            return new MoveResultDto(false, "You can't walk there.", await BuildGameState(character, zone, cancellationToken));

        character.MoveTo(newX, newY);

        // Check for zone connection
        var connections = await _connectionRepo.FindAsync(
            c => c.FromZoneId == zone.Id && c.FromX == newX && c.FromY == newY, cancellationToken);
        var connection = connections.FirstOrDefault();

        string message;
        if (connection is not null)
        {
            var previousZoneId = zone.Id;
            character.ChangeZone(connection.ToZoneId, connection.ToX, connection.ToY, previousZoneId);

            var newZone = await _zoneRepo.GetByIdAsync(connection.ToZoneId, cancellationToken)
                ?? throw new KeyNotFoundException($"Zone with id {connection.ToZoneId} not found.");

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            message = $"You entered {newZone.Name}.";
            return new MoveResultDto(true, message, await BuildGameState(character, newZone, cancellationToken));
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        message = (request.Dx, request.Dy) switch
        {
            (0, -1) => "You walked north.",
            (0, 1) => "You walked south.",
            (1, 0) => "You walked east.",
            (-1, 0) => "You walked west.",
            _ => "You moved."
        };

        // Check for combat encounter
        var combatState = await CheckForEncounter(character, zone, tile, cancellationToken);

        return new MoveResultDto(true, message, await BuildGameState(character, zone, cancellationToken), combatState);
    }

    private async Task<CombatStateDto?> CheckForEncounter(
        Domain.Entities.Character character, Zone zone, Tile tile, CancellationToken ct)
    {
        if (_encounterStore.HasActiveEncounter(character.Id))
            return null;

        var shouldTrigger = tile.TileType == TileType.Encounter
            || (CombatRules.IsDangerousZone(zone.ZoneType) && CombatRules.ShouldTriggerEncounter(_dice));

        if (!shouldTrigger)
            return null;

        var allMonsters = await _monsterRepo.GetAllAsync(ct);
        var monsterList = allMonsters.ToList();
        if (monsterList.Count == 0)
            return null;

        var monsterCount = _dice.Roll(1, 2);
        var selectedMonsterIds = new List<Guid>();
        for (var i = 0; i < monsterCount && i < monsterList.Count; i++)
        {
            var index = _dice.Roll(1, monsterList.Count) - 1;
            selectedMonsterIds.Add(monsterList[index].Id);
        }

        var command = new InitiateCombatCommand(character.Id, selectedMonsterIds);
        return await _mediator.Send(command, ct);
    }

    private async Task<GameStateDto> BuildGameState(Domain.Entities.Character character, Zone zone, CancellationToken ct)
    {
        var tiles = await _tileRepo.FindAsync(t => t.ZoneId == zone.Id, ct);
        var npcs = await _npcRepo.FindAsync(n => n.ZoneId == zone.Id, ct);

        return new GameStateDto(
            character.ToDto(),
            zone.ToDto(),
            tiles.Select(t => t.ToDto()).ToList(),
            npcs.Select(n => n.ToDto()).ToList());
    }
}
