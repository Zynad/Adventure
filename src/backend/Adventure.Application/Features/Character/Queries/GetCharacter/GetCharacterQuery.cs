using MediatR;

namespace Adventure.Application.Features.Character.Queries.GetCharacter;

public record GetCharacterQuery(Guid Id) : IRequest<CharacterDto?>;
