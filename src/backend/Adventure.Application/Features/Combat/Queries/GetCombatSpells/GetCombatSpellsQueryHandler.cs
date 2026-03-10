using Adventure.Domain.Enums;
using Adventure.Domain.Interfaces;
using Adventure.Application.Features.Spell;
using MediatR;
using SpellEntity = Adventure.Domain.Entities.Spell;

namespace Adventure.Application.Features.Combat.Queries.GetCombatSpells;

public class GetCombatSpellsQueryHandler : IRequestHandler<GetCombatSpellsQuery, CombatSpellInfoDto>
{
    private readonly ICombatEncounterStore _encounterStore;
    private readonly IRepository<SpellEntity> _spellRepo;

    public GetCombatSpellsQueryHandler(
        ICombatEncounterStore encounterStore,
        IRepository<SpellEntity> spellRepo)
    {
        _encounterStore = encounterStore;
        _spellRepo = spellRepo;
    }

    public async Task<CombatSpellInfoDto> Handle(GetCombatSpellsQuery request, CancellationToken cancellationToken)
    {
        var encounter = _encounterStore.GetByCharacterId(request.CharacterId)
            ?? throw new KeyNotFoundException("No active combat encounter found.");

        var participant = encounter.Participants
            .FirstOrDefault(p => p.ParticipantType == ParticipantType.Player && p.Id == request.CharacterId)
            ?? throw new KeyNotFoundException("Player not found in encounter.");

        var spells = new List<CombatSpellDto>();
        foreach (var spellId in participant.KnownSpellIds)
        {
            var spell = await _spellRepo.GetByIdAsync(spellId, cancellationToken);
            if (spell is null) continue;

            var canCast = spell.IsCantrip || participant.HasSpellSlot(spell.SpellLevel);

            var targetType = DetermineTargetType(spell);

            spells.Add(new CombatSpellDto(
                spell.Id,
                spell.Name,
                spell.SpellLevel,
                spell.DamageDice,
                spell.HealingDice,
                spell.IsCantrip,
                canCast,
                targetType));
        }

        var slotDtos = participant.SpellSlots
            .Select(s => new SpellSlotDto(s.Level, s.MaxSlots, s.CurrentSlots))
            .ToList();

        return new CombatSpellInfoDto(spells, slotDtos);
    }

    private static string DetermineTargetType(SpellEntity spell)
    {
        if (spell.IsHealingSpell)
            return "ally";

        if (spell.DamageDice is not null)
            return "enemy";

        // Buff spells
        if (spell.Name == "Shield")
            return "self";

        if (spell.Name == "Bless")
            return "ally";

        return "none";
    }
}
