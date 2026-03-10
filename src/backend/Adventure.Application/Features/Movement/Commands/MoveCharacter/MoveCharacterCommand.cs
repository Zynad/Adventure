using MediatR;

namespace Adventure.Application.Features.Movement.Commands.MoveCharacter;

public record MoveCharacterCommand(
    Guid CharacterId,
    int Dx,
    int Dy) : IRequest<MoveResultDto>;
