using Adventure.Domain.Interfaces;
using MediatR;

namespace Adventure.Application.Features.Character.Queries.GetCharacter;

public class GetCharacterQueryHandler : IRequestHandler<GetCharacterQuery, CharacterDto?>
{
    private readonly IRepository<Domain.Entities.Character> _characterRepo;

    public GetCharacterQueryHandler(IRepository<Domain.Entities.Character> characterRepo)
    {
        _characterRepo = characterRepo;
    }

    public async Task<CharacterDto?> Handle(GetCharacterQuery request, CancellationToken cancellationToken)
    {
        var character = await _characterRepo.GetByIdAsync(request.Id, cancellationToken);
        return character?.ToDto();
    }
}
