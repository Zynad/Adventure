using Adventure.Domain.ValueObjects;

namespace Adventure.Application.Features.Spell;

public static class SpellMappingExtensions
{
    public static SpellDto ToDto(this Domain.Entities.Spell spell)
    {
        return new SpellDto(
            spell.Id,
            spell.Name,
            spell.Description,
            spell.SpellLevel,
            (int)spell.School,
            spell.CastingTime,
            spell.Range,
            spell.Duration,
            spell.DamageDice,
            spell.DamageType.HasValue ? (int)spell.DamageType.Value : null,
            spell.HealingDice,
            spell.SavingThrowAbility.HasValue ? (int)spell.SavingThrowAbility.Value : null,
            (int)spell.RequiredClass,
            spell.RequiresAttackRoll,
            spell.IsCantrip);
    }

    public static KnownSpellDto ToKnownSpellDto(this Domain.Entities.Spell spell, bool canCast)
    {
        return new KnownSpellDto(
            spell.Id,
            spell.Name,
            spell.Description,
            spell.SpellLevel,
            (int)spell.School,
            spell.DamageDice,
            spell.DamageType.HasValue ? (int)spell.DamageType.Value : null,
            spell.HealingDice,
            spell.IsCantrip,
            canCast);
    }

    public static SpellSlotDto ToDto(this SpellSlot slot)
    {
        return new SpellSlotDto(slot.Level, slot.MaxSlots, slot.CurrentSlots);
    }
}
