using Adventure.Domain.Entities;
using Adventure.Domain.Interfaces;
using MediatR;

namespace Adventure.Application.Features.World.Queries.GetZoneTiles;

public class GetZoneTilesQueryHandler : IRequestHandler<GetZoneTilesQuery, IReadOnlyList<TileDto>>
{
    private readonly IRepository<Tile> _tileRepo;

    public GetZoneTilesQueryHandler(IRepository<Tile> tileRepo)
    {
        _tileRepo = tileRepo;
    }

    public async Task<IReadOnlyList<TileDto>> Handle(GetZoneTilesQuery request, CancellationToken cancellationToken)
    {
        var tiles = await _tileRepo.FindAsync(t => t.ZoneId == request.ZoneId, cancellationToken);
        return tiles.Select(t => t.ToDto()).ToList();
    }
}
