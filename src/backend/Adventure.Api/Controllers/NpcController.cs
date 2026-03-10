using Adventure.Application.Features.Dialogue;
using Adventure.Application.Features.Dialogue.Commands.AdvanceDialogue;
using Adventure.Application.Features.Dialogue.Queries.GetNpcDialogue;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Adventure.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NpcController : ControllerBase
{
    private readonly IMediator _mediator;

    public NpcController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{npcId:guid}/dialogue")]
    public async Task<ActionResult<DialogueNodeDto>> GetDialogue(Guid npcId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetNpcDialogueQuery(npcId), ct);
        return Ok(result);
    }

    [HttpPost("{npcId:guid}/dialogue/advance")]
    public async Task<ActionResult<DialogueNodeDto>> AdvanceDialogue(
        Guid npcId, AdvanceDialogueCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command with { NpcId = npcId }, ct);
        return Ok(result);
    }
}
