using MediatR;

namespace Adventure.Application.Features.World.Queries.GetZones;

public record GetZonesQuery() : IRequest<IReadOnlyList<ZoneDto>>;
