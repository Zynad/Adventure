namespace Adventure.Infrastructure.Persistence.Seeding;

internal record ZoneSeed(
    Guid Id, string Name, string Description, int ZoneType,
    int Width, int Height, int WorldMapX, int WorldMapY, bool IsDiscovered);

internal record TileSeed(
    Guid Id, Guid ZoneId, int X, int Y, int TileType,
    bool IsWalkable, bool IsInteractable, string? InteractionData, string TilesetSpriteName);

internal record NpcSeed(
    Guid Id, string Name, string Description, Guid ZoneId,
    int PositionX, int PositionY, string? DialogueTree,
    bool IsQuestGiver, bool IsMerchant, Guid? FactionId);

internal record MonsterSeed(
    Guid Id, string Name, string Description, int Level,
    int HitPoints, int ArmorClass,
    int Strength, int Dexterity, int Constitution,
    int Intelligence, int Wisdom, int Charisma,
    decimal ChallengeRating, int ExperienceReward,
    string AttackDice, int DamageType, int AiStrategy, Guid? LootTableId);

internal record ZoneConnectionSeed(
    Guid Id, Guid FromZoneId, Guid ToZoneId,
    int FromX, int FromY, int ToX, int ToY);

internal record SpellSeed(
    Guid Id, string Name, string Description, int SpellLevel,
    int School, string CastingTime, string Range, string Duration,
    string? DamageDice, int? DamageType, string? HealingDice,
    int? SavingThrowAbility, int RequiredClass, bool RequiresAttackRoll);

internal record ItemSeed(
    Guid Id, string Type, string Name, string Description,
    int ItemType, int Rarity, decimal Weight, int BaseValue, bool IsStackable,
    // Equipment fields
    int? EquipmentSlot, int? ArmorBonus, string? DamageDice, int? DamageType,
    int? RequiredLevel, int? RequiredClass, string? StatBonuses,
    // Consumable fields
    string? EffectType, int? EffectValue);
