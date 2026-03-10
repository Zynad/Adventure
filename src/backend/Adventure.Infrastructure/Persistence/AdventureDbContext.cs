using Adventure.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Adventure.Infrastructure.Persistence;

public class AdventureDbContext : DbContext
{
    public AdventureDbContext(DbContextOptions<AdventureDbContext> options) : base(options)
    {
    }

    public DbSet<Character> Characters => Set<Character>();
    public DbSet<Monster> Monsters => Set<Monster>();
    public DbSet<Item> Items => Set<Item>();
    public DbSet<Equipment> Equipment => Set<Equipment>();
    public DbSet<Consumable> Consumables => Set<Consumable>();
    public DbSet<CraftingMaterial> CraftingMaterials => Set<CraftingMaterial>();
    public DbSet<Zone> Zones => Set<Zone>();
    public DbSet<Tile> Tiles => Set<Tile>();
    public DbSet<ZoneConnection> ZoneConnections => Set<ZoneConnection>();
    public DbSet<Npc> Npcs => Set<Npc>();
    public DbSet<Shop> Shops => Set<Shop>();
    public DbSet<ShopInventoryEntry> ShopInventoryEntries => Set<ShopInventoryEntry>();
    public DbSet<Quest> Quests => Set<Quest>();
    public DbSet<QuestObjective> QuestObjectives => Set<QuestObjective>();
    public DbSet<CharacterQuest> CharacterQuests => Set<CharacterQuest>();
    public DbSet<Faction> Factions => Set<Faction>();
    public DbSet<CharacterFactionReputation> CharacterFactionReputations => Set<CharacterFactionReputation>();
    public DbSet<Profession> Professions => Set<Profession>();
    public DbSet<CharacterProfession> CharacterProfessions => Set<CharacterProfession>();
    public DbSet<Recipe> Recipes => Set<Recipe>();
    public DbSet<RecipeIngredient> RecipeIngredients => Set<RecipeIngredient>();
    public DbSet<Spell> Spells => Set<Spell>();
    public DbSet<Companion> Companions => Set<Companion>();
    public DbSet<Pet> Pets => Set<Pet>();
    public DbSet<InventorySlot> InventorySlots => Set<InventorySlot>();
    public DbSet<EquippedItem> EquippedItems => Set<EquippedItem>();
    public DbSet<LootTable> LootTables => Set<LootTable>();
    public DbSet<LootTableEntry> LootTableEntries => Set<LootTableEntry>();
    public DbSet<SaveGame> SaveGames => Set<SaveGame>();
    public DbSet<CharacterKnownSpell> CharacterKnownSpells => Set<CharacterKnownSpell>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AdventureDbContext).Assembly);
    }
}
