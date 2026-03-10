using Adventure.Application.Features.Character;
using Adventure.Application.Features.World;
using Adventure.Domain.Entities;
using Adventure.Domain.Interfaces;
using MediatR;

namespace Adventure.Application.Features.Game.Queries.GetGameState;

public class GetGameStateQueryHandler : IRequestHandler<GetGameStateQuery, GameStateDto>
{
    private readonly IRepository<Domain.Entities.Character> _characterRepo;
    private readonly IRepository<Zone> _zoneRepo;
    private readonly IRepository<Tile> _tileRepo;
    private readonly IRepository<Npc> _npcRepo;

    public GetGameStateQueryHandler(
        IRepository<Domain.Entities.Character> characterRepo,
        IRepository<Zone> zoneRepo,
        IRepository<Tile> tileRepo,
        IRepository<Npc> npcRepo)
    {
        _characterRepo = characterRepo;
        _zoneRepo = zoneRepo;
        _tileRepo = tileRepo;
        _npcRepo = npcRepo;
    }

    public async Task<GameStateDto> Handle(GetGameStateQuery request, CancellationToken cancellationToken)
    {
        var character = await _characterRepo.GetByIdAsync(request.CharacterId, cancellationToken)
            ?? throw new KeyNotFoundException($"Character with id {request.CharacterId} not found.");

        var zone = await _zoneRepo.GetByIdAsync(character.CurrentZoneId, cancellationToken)
            ?? throw new KeyNotFoundException($"Zone with id {character.CurrentZoneId} not found.");

        var tiles = await _tileRepo.FindAsync(t => t.ZoneId == zone.Id, cancellationToken);
        var npcs = await _npcRepo.FindAsync(n => n.ZoneId == zone.Id, cancellationToken);

        return new GameStateDto(
            character.ToDto(),
            zone.ToDto(),
            tiles.Select(t => t.ToDto()).ToList(),
            npcs.Select(n => n.ToDto()).ToList());
    }
}
