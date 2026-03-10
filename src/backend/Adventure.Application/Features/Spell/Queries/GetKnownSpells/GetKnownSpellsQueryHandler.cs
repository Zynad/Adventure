using Adventure.Domain.Entities;
using Adventure.Domain.Interfaces;
using Adventure.Domain.Rules;
using MediatR;

namespace Adventure.Application.Features.Spell.Queries.GetKnownSpells;

public class GetKnownSpellsQueryHandler : IRequestHandler<GetKnownSpellsQuery, CharacterSpellInfoDto>
{
    private readonly IRepository<Domain.Entities.Character> _characterRepo;
    private readonly IRepository<Domain.Entities.Spell> _spellRepo;
    private readonly IRepository<CharacterKnownSpell> _knownSpellRepo;

    public GetKnownSpellsQueryHandler(
        IRepository<Domain.Entities.Character> characterRepo,
        IRepository<Domain.Entities.Spell> spellRepo,
        IRepository<CharacterKnownSpell> knownSpellRepo)
    {
        _characterRepo = characterRepo;
        _spellRepo = spellRepo;
        _knownSpellRepo = knownSpellRepo;
    }

    public async Task<CharacterSpellInfoDto> Handle(GetKnownSpellsQuery request, CancellationToken cancellationToken)
    {
        var character = await _characterRepo.GetByIdAsync(request.CharacterId, cancellationToken)
            ?? throw new KeyNotFoundException($"Character with id {request.CharacterId} not found.");

        if (!SpellSlotTable.IsSpellcaster(character.Class))
            return new CharacterSpellInfoDto([], [], null, null);

        var knownSpellJoins = await _knownSpellRepo.FindAsync(
            ks => ks.CharacterId == request.CharacterId, cancellationToken);

        var slots = character.GetSpellSlots();
        var slotDtos = slots.Select(s => new SpellSlotDto(s.Level, s.MaxSlots, s.CurrentSlots)).ToList();

        var spellcastingAbility = SpellcastingAbilityTable.GetSpellcastingAbility(character.Class);
        int? spellSaveDC = null;
        int? spellAttackBonus = null;

        if (spellcastingAbility.HasValue)
        {
            var mod = character.GetAbilityModifier(spellcastingAbility.Value);
            var prof = character.GetProficiencyBonus();
            spellSaveDC = 8 + prof + mod;
            spellAttackBonus = prof + mod;
        }

        var knownSpells = new List<KnownSpellDto>();
        foreach (var join in knownSpellJoins)
        {
            var spell = await _spellRepo.GetByIdAsync(join.SpellId, cancellationToken);
            if (spell is null) continue;

            var canCast = spell.IsCantrip || slots.Any(s => s.Level >= spell.SpellLevel && s.HasAvailableSlot);
            knownSpells.Add(spell.ToKnownSpellDto(canCast));
        }

        return new CharacterSpellInfoDto(knownSpells, slotDtos, spellSaveDC, spellAttackBonus);
    }
}
