using Adventure.Domain.Entities;
using Adventure.Domain.Enums;
using Adventure.Domain.Rules;
using Adventure.Domain.ValueObjects;

namespace Adventure.Domain.Combat;

public class CombatParticipant
{
    private readonly HashSet<CombatCondition> _activeConditions = [];

    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public ParticipantType ParticipantType { get; private set; }
    public int CurrentHp { get; private set; }
    public int MaxHp { get; private set; }
    public int ArmorClass { get; private set; }
    public int InitiativeRoll { get; private set; }
    public bool IsAlive => CurrentHp > 0;
    public int Strength { get; private set; }
    public int Dexterity { get; private set; }
    public int Constitution { get; private set; }
    public int Intelligence { get; private set; }
    public int Wisdom { get; private set; }
    public int Charisma { get; private set; }
    public int Level { get; private set; }
    public string AttackDice { get; private set; } = string.Empty;
    public DamageType DamageType { get; private set; }
    public AIStrategy? AiStrategy { get; private set; }
    public CharacterClass? CharacterClass { get; private set; }
    public List<SpellSlot> SpellSlots { get; private set; } = [];
    public List<Guid> KnownSpellIds { get; private set; } = [];
    public IReadOnlySet<CombatCondition> ActiveConditions => _activeConditions;

    private CombatParticipant() { }

    public void TakeDamage(int damage)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(damage);
        CurrentHp = Math.Max(0, CurrentHp - damage);
    }

    public void Heal(int amount)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(amount);
        CurrentHp = Math.Min(MaxHp, CurrentHp + amount);
    }

    public void SetInitiative(int roll)
    {
        InitiativeRoll = roll;
    }

    public void ApplyCondition(CombatCondition condition) => _activeConditions.Add(condition);

    public void RemoveCondition(CombatCondition condition) => _activeConditions.Remove(condition);

    public bool HasCondition(CombatCondition condition) => _activeConditions.Contains(condition);

    public void ClearConditions() => _activeConditions.Clear();

    public int GetAbilityModifier(AbilityType ability)
    {
        var score = ability switch
        {
            AbilityType.Strength => Strength,
            AbilityType.Dexterity => Dexterity,
            AbilityType.Constitution => Constitution,
            AbilityType.Intelligence => Intelligence,
            AbilityType.Wisdom => Wisdom,
            AbilityType.Charisma => Charisma,
            _ => throw new ArgumentOutOfRangeException(nameof(ability))
        };
        return AbilityModifierCalculator.Calculate(score);
    }

    public int GetProficiencyBonus() => ProficiencyBonusTable.GetBonus(Level);

    public int GetPassivePerception() => 10 + GetAbilityModifier(AbilityType.Wisdom);

    public bool HasSpellSlot(int spellLevel)
    {
        return SpellSlots.Any(s => s.Level >= spellLevel && s.HasAvailableSlot);
    }

    public void UseSpellSlot(int spellLevel)
    {
        for (var i = 0; i < SpellSlots.Count; i++)
        {
            if (SpellSlots[i].Level >= spellLevel && SpellSlots[i].HasAvailableSlot)
            {
                SpellSlots[i] = SpellSlots[i].UseSlot();
                return;
            }
        }

        throw new InvalidOperationException($"No available spell slot for level {spellLevel}.");
    }

    public int GetSpellcastingAbilityModifier()
    {
        var ability = CharacterClass.HasValue
            ? SpellcastingAbilityTable.GetSpellcastingAbility(CharacterClass.Value)
            : null;

        return ability.HasValue ? GetAbilityModifier(ability.Value) : 0;
    }

    public int GetSpellSaveDC() => 8 + GetProficiencyBonus() + GetSpellcastingAbilityModifier();

    public int GetSpellAttackBonus() => GetProficiencyBonus() + GetSpellcastingAbilityModifier();

    public int GetEffectiveAC()
    {
        var ac = ArmorClass;
        if (HasCondition(CombatCondition.Shielded))
            ac += 5;
        return ac;
    }

    public static CombatParticipant FromCharacter(Character character)
    {
        return new CombatParticipant
        {
            Id = character.Id,
            Name = character.Name,
            ParticipantType = ParticipantType.Player,
            CurrentHp = character.CurrentHitPoints,
            MaxHp = character.MaxHitPoints,
            ArmorClass = character.ArmorClass,
            Strength = character.Strength,
            Dexterity = character.Dexterity,
            Constitution = character.Constitution,
            Intelligence = character.Intelligence,
            Wisdom = character.Wisdom,
            Charisma = character.Charisma,
            Level = character.Level,
            AttackDice = "1d4",
            DamageType = DamageType.Bludgeoning,
            CharacterClass = character.Class,
            SpellSlots = character.GetSpellSlots(),
            KnownSpellIds = character.KnownSpells.Select(ks => ks.SpellId).ToList()
        };
    }

    public static CombatParticipant FromMonster(Monster monster, Guid instanceId)
    {
        return new CombatParticipant
        {
            Id = instanceId,
            Name = monster.Name,
            ParticipantType = ParticipantType.Enemy,
            CurrentHp = monster.HitPoints,
            MaxHp = monster.HitPoints,
            ArmorClass = monster.ArmorClass,
            Strength = monster.Strength,
            Dexterity = monster.Dexterity,
            Constitution = monster.Constitution,
            Intelligence = monster.Intelligence,
            Wisdom = monster.Wisdom,
            Charisma = monster.Charisma,
            Level = monster.Level,
            AttackDice = monster.AttackDice,
            DamageType = monster.DamageType,
            AiStrategy = monster.AIStrategy
        };
    }
}
