using Adventure.Application.Features.Combat;
using Adventure.Application.Features.Game;

namespace Adventure.Application.Features.Movement;

public record MoveResultDto(
    bool Success,
    string? Message,
    GameStateDto GameState,
    CombatStateDto? CombatEncounter = null);
