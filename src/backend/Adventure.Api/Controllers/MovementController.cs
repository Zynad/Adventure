using Adventure.Application.Features.Movement;
using Adventure.Application.Features.Movement.Commands.MoveCharacter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Adventure.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovementController : ControllerBase
{
    private readonly IMediator _mediator;

    public MovementController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("move")]
    public async Task<ActionResult<MoveResultDto>> Move(
        MoveCharacterCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return Ok(result);
    }
}
