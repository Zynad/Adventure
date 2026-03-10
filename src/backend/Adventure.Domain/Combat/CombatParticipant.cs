using Adventure.Domain.Entities;
using Adventure.Domain.Enums;
using Adventure.Domain.Rules;

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
    public int Wisdom { get; private set; }
    public int Level { get; private set; }
    public string AttackDice { get; private set; } = string.Empty;
    public DamageType DamageType { get; private set; }
    public AIStrategy? AiStrategy { get; private set; }
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
            AbilityType.Wisdom => Wisdom,
            _ => Strength
        };
        return AbilityModifierCalculator.Calculate(score);
    }

    public int GetProficiencyBonus() => ProficiencyBonusTable.GetBonus(Level);

    public int GetPassivePerception() => 10 + GetAbilityModifier(AbilityType.Wisdom);

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
            Wisdom = character.Wisdom,
            Level = character.Level,
            AttackDice = "1d4",
            DamageType = DamageType.Bludgeoning
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
            Wisdom = monster.Wisdom,
            Level = monster.Level,
            AttackDice = monster.AttackDice,
            DamageType = monster.DamageType,
            AiStrategy = monster.AIStrategy
        };
    }
}
