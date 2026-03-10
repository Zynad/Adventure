using System.Text.Json;
using Adventure.Domain.Entities;
using Adventure.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Adventure.Infrastructure.Persistence.Seeding;

public static class DatabaseSeeder
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static async Task SeedAsync(AdventureDbContext context, string dataPath)
    {
        if (await context.Zones.AnyAsync())
            return;

        await SeedZonesAsync(context, dataPath);
        await SeedTilesAsync(context, dataPath);
        await SeedZoneConnectionsAsync(context, dataPath);
        await SeedNpcsAsync(context, dataPath);
        await SeedMonstersAsync(context, dataPath);
        await SeedItemsAsync(context, dataPath);

        await context.SaveChangesAsync();
    }

    private static async Task SeedZonesAsync(AdventureDbContext context, string dataPath)
    {
        var json = await File.ReadAllTextAsync(Path.Combine(dataPath, "zones.json"));
        var seeds = JsonSerializer.Deserialize<List<ZoneSeed>>(json, JsonOptions)!;

        foreach (var s in seeds)
        {
            var zone = Zone.CreateForSeed(
                s.Id, s.Name, s.Description, (ZoneType)s.ZoneType,
                s.Width, s.Height, s.WorldMapX, s.WorldMapY, s.IsDiscovered);
            await context.Zones.AddAsync(zone);
        }
    }

    private static async Task SeedTilesAsync(AdventureDbContext context, string dataPath)
    {
        var json = await File.ReadAllTextAsync(Path.Combine(dataPath, "tiles.json"));
        var seeds = JsonSerializer.Deserialize<List<TileSeed>>(json, JsonOptions)!;

        foreach (var s in seeds)
        {
            var tile = Tile.CreateForSeed(
                s.Id, s.ZoneId, s.X, s.Y, (TileType)s.TileType,
                s.IsWalkable, s.IsInteractable, s.InteractionData, s.TilesetSpriteName);
            await context.Tiles.AddAsync(tile);
        }
    }

    private static async Task SeedZoneConnectionsAsync(AdventureDbContext context, string dataPath)
    {
        var filePath = Path.Combine(dataPath, "zone-connections.json");
        if (!File.Exists(filePath))
            return;

        var json = await File.ReadAllTextAsync(filePath);
        var seeds = JsonSerializer.Deserialize<List<ZoneConnectionSeed>>(json, JsonOptions)!;

        foreach (var s in seeds)
        {
            var connection = ZoneConnection.CreateForSeed(
                s.Id, s.FromZoneId, s.ToZoneId,
                s.FromX, s.FromY, s.ToX, s.ToY);
            await context.ZoneConnections.AddAsync(connection);
        }
    }

    private static async Task SeedNpcsAsync(AdventureDbContext context, string dataPath)
    {
        var json = await File.ReadAllTextAsync(Path.Combine(dataPath, "npcs.json"));
        var seeds = JsonSerializer.Deserialize<List<NpcSeed>>(json, JsonOptions)!;

        foreach (var s in seeds)
        {
            var npc = Npc.CreateForSeed(
                s.Id, s.Name, s.Description, s.ZoneId,
                s.PositionX, s.PositionY, s.DialogueTree,
                s.IsQuestGiver, s.IsMerchant, s.FactionId);
            await context.Npcs.AddAsync(npc);
        }
    }

    private static async Task SeedMonstersAsync(AdventureDbContext context, string dataPath)
    {
        var json = await File.ReadAllTextAsync(Path.Combine(dataPath, "monsters.json"));
        var seeds = JsonSerializer.Deserialize<List<MonsterSeed>>(json, JsonOptions)!;

        foreach (var s in seeds)
        {
            var monster = Monster.CreateForSeed(
                s.Id, s.Name, s.Description, s.Level,
                s.HitPoints, s.ArmorClass,
                s.Strength, s.Dexterity, s.Constitution,
                s.Intelligence, s.Wisdom, s.Charisma,
                s.ChallengeRating, s.ExperienceReward,
                s.AttackDice, (DamageType)s.DamageType, (AIStrategy)s.AiStrategy, s.LootTableId);
            await context.Monsters.AddAsync(monster);
        }
    }

    private static async Task SeedItemsAsync(AdventureDbContext context, string dataPath)
    {
        var json = await File.ReadAllTextAsync(Path.Combine(dataPath, "items.json"));
        var seeds = JsonSerializer.Deserialize<List<ItemSeed>>(json, JsonOptions)!;

        foreach (var s in seeds)
        {
            if (s.Type == "Equipment")
            {
                var item = Equipment.CreateForSeed(
                    s.Id, s.Name, s.Description, (ItemRarity)s.Rarity,
                    s.Weight, s.BaseValue, s.IsStackable,
                    (EquipmentSlotType)s.EquipmentSlot!.Value, s.ArmorBonus, s.DamageDice,
                    s.DamageType.HasValue ? (DamageType)s.DamageType.Value : null,
                    s.RequiredLevel ?? 1, s.RequiredClass.HasValue ? (CharacterClass)s.RequiredClass.Value : null,
                    s.StatBonuses);
                await context.Items.AddAsync(item);
            }
            else if (s.Type == "Consumable")
            {
                var item = Consumable.CreateForSeed(
                    s.Id, s.Name, s.Description, (ItemRarity)s.Rarity,
                    s.Weight, s.BaseValue, s.IsStackable,
                    s.EffectType!, s.EffectValue!.Value);
                await context.Items.AddAsync(item);
            }
        }
    }
}
