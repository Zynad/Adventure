namespace Adventure.Application.Features.Spell;

public record SpellDto(
    Guid Id,
    string Name,
    string Description,
    int SpellLevel,
    int School,
    string CastingTime,
    string Range,
    string Duration,
    string? DamageDice,
    int? DamageType,
    string? HealingDice,
    int? SavingThrowAbility,
    int RequiredClass,
    bool RequiresAttackRoll,
    bool IsCantrip);

public record KnownSpellDto(
    Guid SpellId,
    string Name,
    string Description,
    int SpellLevel,
    int School,
    string? DamageDice,
    int? DamageType,
    string? HealingDice,
    bool IsCantrip,
    bool CanCast);

public record SpellSlotDto(int Level, int MaxSlots, int CurrentSlots);

public record CharacterSpellInfoDto(
    IReadOnlyList<KnownSpellDto> KnownSpells,
    IReadOnlyList<SpellSlotDto> SpellSlots,
    int? SpellSaveDC,
    int? SpellAttackBonus);
