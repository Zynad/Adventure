using MediatR;

namespace Adventure.Application.Features.Game.Queries.GetGameState;

public record GetGameStateQuery(Guid CharacterId) : IRequest<GameStateDto>;
