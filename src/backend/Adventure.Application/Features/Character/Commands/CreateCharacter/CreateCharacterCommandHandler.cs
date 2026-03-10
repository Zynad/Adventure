using Adventure.Domain.Entities;
using Adventure.Domain.Interfaces;
using Adventure.Domain.Rules;
using MediatR;
using SpellEntity = Adventure.Domain.Entities.Spell;

namespace Adventure.Application.Features.Character.Commands.CreateCharacter;

public class CreateCharacterCommandHandler : IRequestHandler<CreateCharacterCommand, CharacterDto>
{
    private readonly IRepository<Domain.Entities.Character> _characterRepo;
    private readonly IRepository<Zone> _zoneRepo;
    private readonly IRepository<SpellEntity> _spellRepo;
    private readonly IRepository<CharacterKnownSpell> _knownSpellRepo;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCharacterCommandHandler(
        IRepository<Domain.Entities.Character> characterRepo,
        IRepository<Zone> zoneRepo,
        IRepository<SpellEntity> spellRepo,
        IRepository<CharacterKnownSpell> knownSpellRepo,
        IUnitOfWork unitOfWork)
    {
        _characterRepo = characterRepo;
        _zoneRepo = zoneRepo;
        _spellRepo = spellRepo;
        _knownSpellRepo = knownSpellRepo;
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

        // Auto-assign starting spells for spellcasting classes
        var startingSpellNames = StartingSpellsTable.GetStartingSpellNames(request.CharacterClass);
        if (startingSpellNames.Count > 0)
        {
            var allSpells = await _spellRepo.GetAllAsync(cancellationToken);
            foreach (var spellName in startingSpellNames)
            {
                var spell = allSpells.FirstOrDefault(s => s.Name == spellName && (int)s.RequiredClass == (int)request.CharacterClass);
                if (spell is not null)
                {
                    var knownSpell = CharacterKnownSpell.Create(character.Id, spell.Id);
                    await _knownSpellRepo.AddAsync(knownSpell, cancellationToken);
                }
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return character.ToDto();
    }
}
