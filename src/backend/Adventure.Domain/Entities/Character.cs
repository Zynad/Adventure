using System.Text.Json;
using Adventure.Domain.Enums;
using Adventure.Domain.Events;
using Adventure.Domain.Rules;
using Adventure.Domain.ValueObjects;

namespace Adventure.Domain.Entities;

public class Character : IHasDomainEvents
{
    private readonly List<DomainEvent> _domainEvents = [];

    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void ClearDomainEvents() => _domainEvents.Clear();

    protected void RaiseDomainEvent(DomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public Race Race { get; private set; }
    public CharacterClass Class { get; private set; }
    public int Level { get; private set; } = 1;
    public int ExperiencePoints { get; private set; }
    public int CurrentHitPoints { get; private set; }
    public int MaxHitPoints { get; private set; }
    public int ArmorClass { get; private set; }
    public int Strength { get; private set; }
    public int Dexterity { get; private set; }
    public int Constitution { get; private set; }
    public int Intelligence { get; private set; }
    public int Wisdom { get; private set; }
    public int Charisma { get; private set; }
    public int Gold { get; private set; }
    public Guid CurrentZoneId { get; private set; }
    public int PositionX { get; private set; }
    public int PositionY { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // Spell slots (JSON-serialized)
    public string? SpellSlotsJson { get; private set; }

    // Navigation properties
    public ICollection<InventorySlot> Inventory { get; private set; } = new List<InventorySlot>();
    public ICollection<EquippedItem> Equipment { get; private set; } = new List<EquippedItem>();
    public ICollection<CharacterKnownSpell> KnownSpells { get; private set; } = new List<CharacterKnownSpell>();
    public ICollection<CharacterQuest> Quests { get; private set; } = new List<CharacterQuest>();
    public ICollection<CharacterFactionReputation> Reputations { get; private set; } = new List<CharacterFactionReputation>();
    public ICollection<CharacterProfession> Professions { get; private set; } = new List<CharacterProfession>();

    protected Character() { }

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

    public bool CanLevelUp()
    {
        if (Level >= 20)
            return false;

        return ExperiencePoints >= ExperienceTable.GetRequiredXp(Level + 1);
    }

    public void GainExperience(int xp)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(xp);

        ExperiencePoints += xp;
        UpdatedAt = DateTime.UtcNow;
    }

    public void TakeDamage(int damage)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(damage);

        CurrentHitPoints = Math.Max(0, CurrentHitPoints - damage);
        UpdatedAt = DateTime.UtcNow;

        if (CurrentHitPoints == 0)
            RaiseDomainEvent(new CharacterDiedEvent(Id));
    }

    public void Heal(int amount)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);

        CurrentHitPoints = Math.Min(MaxHitPoints, CurrentHitPoints + amount);
        UpdatedAt = DateTime.UtcNow;
    }

    public void MoveTo(int newX, int newY)
    {
        PositionX = newX;
        PositionY = newY;
        UpdatedAt = DateTime.UtcNow;
    }

    public List<SpellSlot> GetSpellSlots()
    {
        if (SpellSlotsJson is not null)
            return JsonSerializer.Deserialize<List<SpellSlot>>(SpellSlotsJson) ?? [];

        return BuildSpellSlotsFromTable();
    }

    public void UseSpellSlot(int spellLevel)
    {
        var slots = GetSpellSlots();
        for (var i = 0; i < slots.Count; i++)
        {
            if (slots[i].Level >= spellLevel && slots[i].HasAvailableSlot)
            {
                slots[i] = slots[i].UseSlot();
                SpellSlotsJson = JsonSerializer.Serialize(slots);
                UpdatedAt = DateTime.UtcNow;
                return;
            }
        }

        throw new InvalidOperationException($"No available spell slot for level {spellLevel}.");
    }

    public void RestoreAllSpellSlots()
    {
        var slots = BuildSpellSlotsFromTable();
        SpellSlotsJson = JsonSerializer.Serialize(slots);
        UpdatedAt = DateTime.UtcNow;
    }

    public void InitializeSpellSlots()
    {
        if (!SpellSlotTable.IsSpellcaster(Class))
            return;

        var slots = BuildSpellSlotsFromTable();
        SpellSlotsJson = JsonSerializer.Serialize(slots);
    }

    public void SetSpellSlotsFromList(List<SpellSlot> slots)
    {
        SpellSlotsJson = JsonSerializer.Serialize(slots);
        UpdatedAt = DateTime.UtcNow;
    }

    private List<SpellSlot> BuildSpellSlotsFromTable()
    {
        var maxSlots = SpellSlotTable.GetMaxSlots(Class, Level);
        return maxSlots
            .OrderBy(kvp => kvp.Key)
            .Select(kvp => new SpellSlot(kvp.Key, kvp.Value, kvp.Value))
            .ToList();
    }

    public void ChangeZone(Guid newZoneId, int spawnX, int spawnY, Guid previousZoneId)
    {
        CurrentZoneId = newZoneId;
        PositionX = spawnX;
        PositionY = spawnY;
        UpdatedAt = DateTime.UtcNow;
        RaiseDomainEvent(new ZoneEnteredEvent(Id, newZoneId, previousZoneId));
    }

    public static Character Create(
        string name,
        Race race,
        CharacterClass characterClass,
        int str,
        int dex,
        int con,
        int intel,
        int wis,
        int cha,
        Guid startingZoneId)
    {
        var conModifier = AbilityModifierCalculator.Calculate(con);
        var baseHp = 10 + conModifier; // Simplified starting HP

        var character = new Character
        {
            Id = Guid.NewGuid(),
            Name = name,
            Race = race,
            Class = characterClass,
            Level = 1,
            ExperiencePoints = 0,
            Strength = str,
            Dexterity = dex,
            Constitution = con,
            Intelligence = intel,
            Wisdom = wis,
            Charisma = cha,
            MaxHitPoints = baseHp,
            CurrentHitPoints = baseHp,
            ArmorClass = 10 + AbilityModifierCalculator.Calculate(dex),
            Gold = 0,
            CurrentZoneId = startingZoneId,
            PositionX = 0,
            PositionY = 0,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        character.InitializeSpellSlots();
        return character;
    }
}
