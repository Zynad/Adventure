using Adventure.Domain.Entities;
using Adventure.Domain.Interfaces;
using Adventure.Domain.Rules;
using MediatR;

namespace Adventure.Application.Features.Spell.Commands.LearnSpell;

public class LearnSpellCommandHandler : IRequestHandler<LearnSpellCommand, KnownSpellDto>
{
    private readonly IRepository<Domain.Entities.Character> _characterRepo;
    private readonly IRepository<Domain.Entities.Spell> _spellRepo;
    private readonly IRepository<CharacterKnownSpell> _knownSpellRepo;
    private readonly IUnitOfWork _unitOfWork;

    public LearnSpellCommandHandler(
        IRepository<Domain.Entities.Character> characterRepo,
        IRepository<Domain.Entities.Spell> spellRepo,
        IRepository<CharacterKnownSpell> knownSpellRepo,
        IUnitOfWork unitOfWork)
    {
        _characterRepo = characterRepo;
        _spellRepo = spellRepo;
        _knownSpellRepo = knownSpellRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<KnownSpellDto> Handle(LearnSpellCommand request, CancellationToken cancellationToken)
    {
        var character = await _characterRepo.GetByIdAsync(request.CharacterId, cancellationToken)
            ?? throw new KeyNotFoundException($"Character with id {request.CharacterId} not found.");

        var spell = await _spellRepo.GetByIdAsync(request.SpellId, cancellationToken)
            ?? throw new KeyNotFoundException($"Spell with id {request.SpellId} not found.");

        if (!SpellSlotTable.IsSpellcaster(character.Class))
            throw new InvalidOperationException($"{character.Name}'s class cannot learn spells.");

        if ((int)spell.RequiredClass != (int)character.Class)
            throw new InvalidOperationException($"{character.Name} cannot learn {spell.Name} (requires a different class).");

        var existing = await _knownSpellRepo.FindAsync(
            ks => ks.CharacterId == request.CharacterId && ks.SpellId == request.SpellId, cancellationToken);

        if (existing.Count > 0)
            throw new InvalidOperationException($"{character.Name} already knows {spell.Name}.");

        var knownSpell = CharacterKnownSpell.Create(character.Id, spell.Id);
        await _knownSpellRepo.AddAsync(knownSpell, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var slots = character.GetSpellSlots();
        var canCast = spell.IsCantrip || slots.Any(s => s.Level >= spell.SpellLevel && s.HasAvailableSlot);

        return spell.ToKnownSpellDto(canCast);
    }
}
