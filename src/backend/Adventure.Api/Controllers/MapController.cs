using Adventure.Application.Features.World;
using Adventure.Application.Features.World.Queries.GetZones;
using Adventure.Application.Features.World.Queries.GetZoneTiles;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Adventure.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MapController : ControllerBase
{
    private readonly IMediator _mediator;

    public MapController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("zones")]
    public async Task<ActionResult<IReadOnlyList<ZoneDto>>> GetZones(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetZonesQuery(), ct);
        return Ok(result);
    }

    [HttpGet("zones/{id:guid}/tiles")]
    public async Task<ActionResult<IReadOnlyList<TileDto>>> GetZoneTiles(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetZoneTilesQuery(id), ct);
        return Ok(result);
    }
}
