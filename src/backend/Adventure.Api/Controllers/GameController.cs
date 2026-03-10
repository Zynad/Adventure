using Adventure.Application.Features.Game;
using Adventure.Application.Features.Game.Queries.GetGameState;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Adventure.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController : ControllerBase
{
    private readonly IMediator _mediator;

    public GameController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("state/{characterId:guid}")]
    public async Task<ActionResult<GameStateDto>> GetState(Guid characterId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetGameStateQuery(characterId), ct);
        return Ok(result);
    }
}
