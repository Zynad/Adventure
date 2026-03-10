using Adventure.Domain.Entities;
using Adventure.Domain.Interfaces;
using MediatR;

namespace Adventure.Application.Features.World.Queries.GetZones;

public class GetZonesQueryHandler : IRequestHandler<GetZonesQuery, IReadOnlyList<ZoneDto>>
{
    private readonly IRepository<Zone> _zoneRepo;

    public GetZonesQueryHandler(IRepository<Zone> zoneRepo)
    {
        _zoneRepo = zoneRepo;
    }

    public async Task<IReadOnlyList<ZoneDto>> Handle(GetZonesQuery request, CancellationToken cancellationToken)
    {
        var zones = await _zoneRepo.GetAllAsync(cancellationToken);
        return zones.Select(z => z.ToDto()).ToList();
    }
}
