using MediatR;

namespace Adventure.Application.Features.World.Queries.GetZoneTiles;

public record GetZoneTilesQuery(Guid ZoneId) : IRequest<IReadOnlyList<TileDto>>;
