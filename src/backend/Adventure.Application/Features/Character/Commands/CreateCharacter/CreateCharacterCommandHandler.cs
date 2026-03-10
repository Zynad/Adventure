using Adventure.Domain.Entities;
using Adventure.Domain.Interfaces;
using MediatR;

namespace Adventure.Application.Features.Character.Commands.CreateCharacter;

public class CreateCharacterCommandHandler : IRequestHandler<CreateCharacterCommand, CharacterDto>
{
    private readonly IRepository<Domain.Entities.Character> _characterRepo;
    private readonly IRepository<Zone> _zoneRepo;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCharacterCommandHandler(
        IRepository<Domain.Entities.Character> characterRepo,
        IRepository<Zone> zoneRepo,
        IUnitOfWork unitOfWork)
    {
        _characterRepo = characterRepo;
        _zoneRepo = zoneRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<CharacterDto> Handle(CreateCharacterCommand request, CancellationToken cancellationToken)
    {
        var zones = await _zoneRepo.GetAllAsync(cancellationToken);
        var startingZone = zones.FirstOrDefault()
            ?? throw new InvalidOperationException("No zones found. Database may not be seeded.");

        var character = Domain.Entities.Character.Create(
            request.Name,
            request.Race,
            request.CharacterClass,
            request.Strength,
            request.Dexterity,
            request.Constitution,
            request.Intelligence,
            request.Wisdom,
            request.Charisma,
            startingZone.Id);

        await _characterRepo.AddAsync(character, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return character.ToDto();
    }
}
