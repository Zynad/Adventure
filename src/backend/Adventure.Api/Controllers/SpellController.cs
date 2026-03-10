using Adventure.Application.Features.Spell;
using Adventure.Application.Features.Spell.Commands.LearnSpell;
using Adventure.Application.Features.Spell.Queries.GetKnownSpells;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Adventure.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SpellController : ControllerBase
{
    private readonly IMediator _mediator;

    public SpellController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("known/{characterId:guid}")]
    public async Task<ActionResult<CharacterSpellInfoDto>> GetKnownSpells(
        Guid characterId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetKnownSpellsQuery(characterId), ct);
        return Ok(result);
    }

    [HttpPost("learn")]
    public async Task<ActionResult<KnownSpellDto>> LearnSpell(
        LearnSpellCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return Ok(result);
    }
}
