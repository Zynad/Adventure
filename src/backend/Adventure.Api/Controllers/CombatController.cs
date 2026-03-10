using Adventure.Application.Features.Combat;
using Adventure.Application.Features.Combat.Commands.CastSpell;
using Adventure.Application.Features.Combat.Commands.ExecuteAttack;
using Adventure.Application.Features.Combat.Commands.ExecuteCombatAction;
using Adventure.Application.Features.Combat.Commands.EndTurn;
using Adventure.Application.Features.Combat.Commands.InitiateCombat;
using Adventure.Application.Features.Combat.Commands.UseItemInCombat;
using Adventure.Application.Features.Combat.Queries.GetCombatConsumables;
using Adventure.Application.Features.Combat.Queries.GetCombatSpells;
using Adventure.Application.Features.Combat.Queries.GetCombatState;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Adventure.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CombatController : ControllerBase
{
    private readonly IMediator _mediator;

    public CombatController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("initiate")]
    public async Task<ActionResult<CombatStateDto>> Initiate(
        InitiateCombatCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return Ok(result);
    }

    [HttpPost("attack")]
    public async Task<ActionResult<CombatActionResultDto>> Attack(
        ExecuteAttackCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return Ok(result);
    }

    [HttpPost("action")]
    public async Task<ActionResult<CombatActionResultDto>> Action(
        ExecuteCombatActionCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return Ok(result);
    }

    [HttpPost("end-turn")]
    public async Task<ActionResult<CombatStateDto>> EndTurn(
        EndTurnCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return Ok(result);
    }

    [HttpPost("use-item")]
    public async Task<ActionResult<CombatActionResultDto>> UseItem(
        UseItemInCombatCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return Ok(result);
    }

    [HttpGet("consumables/{characterId:guid}")]
    public async Task<ActionResult<IReadOnlyList<CombatConsumableDto>>> GetConsumables(
        Guid characterId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetCombatConsumablesQuery(characterId), ct);
        return Ok(result);
    }

    [HttpPost("cast-spell")]
    public async Task<ActionResult<CombatActionResultDto>> CastSpell(
        CastSpellCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return Ok(result);
    }

    [HttpGet("spells/{characterId:guid}")]
    public async Task<ActionResult<CombatSpellInfoDto>> GetCombatSpells(
        Guid characterId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetCombatSpellsQuery(characterId), ct);
        return Ok(result);
    }

    [HttpGet("state/{characterId:guid}")]
    public async Task<ActionResult<CombatStateDto>> GetState(
        Guid characterId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetCombatStateQuery(characterId), ct);
        return Ok(result);
    }
}
