using Adventure.Application.Features.Character;
using Adventure.Application.Features.Character.Commands.CreateCharacter;
using Adventure.Application.Features.Character.Queries.GetCharacter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Adventure.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CharacterController : ControllerBase
{
    private readonly IMediator _mediator;

    public CharacterController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<CharacterDto>> Create(
        CreateCharacterCommand command,
        CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CharacterDto>> GetById(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetCharacterQuery(id), ct);
        if (result is null)
            return NotFound();
        return Ok(result);
    }
}
