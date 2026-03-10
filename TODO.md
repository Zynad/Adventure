# Adventure — Roadmap & TODO

## Fas 1: Foundation — "Walking Skeleton"

### Project Setup
- [x] Monorepo-struktur (src/backend, src/frontend, assets)
- [x] .gitignore, .editorconfig
- [x] Backend: .NET 10 solution (.slnx) med Clean Architecture (Domain, Application, Infrastructure, Api)
- [x] Backend: Projektreferenser och NuGet-paket (EF Core, MediatR, FluentValidation)
- [x] Backend: nuget.config (lokal, enbart nuget.org)
- [x] Frontend: Vite + React + TypeScript
- [x] Frontend: Tailwind CSS, Zustand, React Router, Axios
- [x] CLAUDE.md (root, backend, frontend)
- [x] TODO.md
- [x] Docker-compose + Dockerfiles
- [x] Test-projekt (xUnit)

### Domain Layer
- [x] Enums (CharacterClass, Race, DamageType, ItemRarity, etc.)
- [x] Value Objects (AbilityScores, DiceRoll, Position, GoldAmount)
- [x] Interfaces (IRepository, IUnitOfWork, ICombatAI, IDiceService)
- [x] Rules (ExperienceTable, AbilityModifier, ProficiencyBonus)
- [x] Entities — skelett (Character, Monster, Item, Zone, Tile, Npc, etc.)
- [x] Domain Events (CombatStarted, LeveledUp, QuestCompleted, etc.)

### Infrastructure Layer
- [x] AdventureDbContext
- [x] Entity Type Configurations
- [x] Initial migration
- [x] Generic Repository implementation
- [x] UnitOfWork implementation
- [x] Database seeder med JSON seed data
- [x] DiceService implementation
- [x] DependencyInjection.cs

### Application Layer
- [x] Character feature (CreateCharacter command/handler)
- [x] World feature (GetZoneMap, GetZoneTiles queries)
- [x] Game feature (GetGameState query)
- [x] Mapping profiles (Entity ↔ DTO)

### Api Layer
- [x] Program.cs med DI, CORS, middleware
- [x] CharacterController (POST create, GET by id)
- [x] MapController (GET world, GET zone, GET tiles)
- [x] GameController (GET state)
- [x] ExceptionHandlingMiddleware

### Frontend — Fas 1
- [x] Grundläggande routing (/, /create-character, /game)
- [x] Character creation wizard (race, class, name, ability scores)
- [x] Basic tile map renderer (CSS grid)
- [x] Narrative panel (text + NPC-lista)
- [x] API-klient (character, map, game endpoints)
- [x] Game view layout (karta + narrative + sidebar)

### Docker
- [x] docker-compose.yml
- [x] Backend Dockerfile
- [x] Frontend Dockerfile (nginx)

---

## Fas 1.1: Playable Core — "Gå runt och utforska"

### Förflyttning
- [x] Backend: MoveCharacter command (validera walkability, zonbounds)
- [x] Backend: ChangeZone command (vid ZoneConnection-tiles)
- [x] Api: MovementController (POST move, POST change-zone)
- [x] Frontend: Tangentbordsinput (WASD / piltangenter)
- [x] Frontend: Klick-på-tile för att flytta (adjacenta tiles)
- [x] Frontend: Smooth kamera/scroll om kartan är större än viewport
- [x] Raisa ZoneEnteredEvent vid zonbyte

### NPC-interaktion & Dialogsystem
- [x] Domain: DialogueNode, DialogueOption modeller (JSON-baserade dialogträd)
- [x] Backend: GetNpcDialogue query (hämta dialogträd för NPC)
- [x] Backend: AdvanceDialogue command (skicka valt alternativ, få nästa nod)
- [x] Api: NpcController (GET dialogue, POST advance)
- [x] Frontend: NPC-klick (interagera om adjacent)
- [x] Frontend: DialogModal komponent (NPC-porträtt/namn, text, valbara alternativ)
- [x] Seed data: Dialogträd för Elder Mirael och Torvin the Smith

### Ability Score Validation (Point Buy)
- [x] Backend: PointBuyValidator (27 poäng, scores 8-15, kostnadstabell)
- [x] Backend: Uppdatera CreateCharacterCommandValidator med point buy-regler
- [x] Frontend: Point Buy UI (visa tillgängliga poäng, kostnad per score)
- [x] Frontend: Visa totala poäng och återstående budget i realtid

### How to Play
- [x] Frontend: HowToPlayModal komponent (öppnas via ?-knapp i game view)
- [x] Innehåll: Kontroller (WASD/pilar), interaktion (klick NPC), spelmekanik-översikt

### Polish
- [x] Frontend: Narrativpanelen visar händelser ("Du gick norrut", "Du pratade med Elder Mirael")
- [x] Frontend: Visuell indikator för interagerbara tiles/NPCs

---

## Fas 2: Combat System

> *Inspiration: D&D 5e regler, Pokemon-stil separat combat-vy (party vs group, sida vid sida)*

### Fas 2.1: Combat Core — "Slå ett slag"

Grundläggande stridsmotorik: initiativ, turordning, melee-attack, skada och dödsfall.

- [x] Domain: CombatEncounter klass (in-memory, ej Entity — håller CombatParticipants, initiativordning, aktiv tur, runda)
- [x] Domain: CombatParticipant (Id, Name, ParticipantType, CurrentHp, MaxHp, AC, InitiativeRoll, IsAlive)
- [x] Domain: CombatRules statisk klass — AttackRoll (d20 + ability mod + proficiency vs AC), DamageRoll (DiceRoll från Equipment/Monster), CriticalHit (natural 20 = dubbla damage dice)
- [x] Application: Features/Combat/ — InitiateCombatCommand (characterId, monsterIds) → skapa CombatEncounter, rulla initiativ (d20 + DEX mod), returnera CombatStateDto
- [x] Application: Features/Combat/ — ExecuteAttackCommand (characterId, targetId) → attackrulle, skaderulle, returnera CombatActionResultDto med tärningsresultat
- [x] Application: Features/Combat/ — GetCombatStateQuery (characterId) → alla deltagare, turordning, aktiv tur, combat log
- [x] Application: CombatStateDto, CombatParticipantDto, CombatActionResultDto (records)
- [x] Application: In-memory CombatEncounterStore (ConcurrentDictionary — ett encounter per character)
- [x] Api: CombatController — POST initiate, POST attack, GET state
- [x] Frontend: CombatView (Pokemon-stil: party vänster, fiender höger, HP bars, turordning överst)
- [x] Frontend: Attack-knapp med target selection (klicka på fiende)
- [x] Frontend: Combat log panel (scrollbar text med händelser)
- [x] Trigger: Strid startar vid encounter-tiles i farliga zoner (+ 15% slumpchans)

### Fas 2.2: Combat Actions & Turns — "Mer än bara hugg"

Alla CombatActionType-val: Dodge, Dash, Disengage, Help, Hide, UseItem. Monster-turer.

- [x] Domain: CombatRules — Dodge (advantage mot attacker mot dig till nästa tur)
- [x] Domain: CombatRules — Disengage (inga opportunity attacks), Help (ge ally advantage), Hide (Stealth check vs passive Perception)
- [x] Domain: Advantage/Disadvantage-system (rulla 2d20, ta högsta/lägsta)
- [x] Application: ExecuteCombatActionCommand (characterId, CombatActionType, targetId?) — generaliserad action handler
- [x] Application: EndTurnCommand — avancera turordning, monster-turer via ICombatAI (placeholder: alla monster attackerar)
- [x] Application: UseItemInCombatCommand (characterId, itemId) — använd Consumable (t.ex. Health Potion)
- [x] Api: Utöka CombatController — POST action, POST end-turn, POST use-item
- [x] Frontend: Action-meny (Attack, Dodge, Disengage, Help, Hide, Use Item)
- [x] Frontend: Target selection UI (klicka fiende/ally)
- [x] Frontend: Combat log med färger (grön = healing, röd = damage, gul = miss)
- [x] Frontend: Turordningsindikator (highlighta aktiv deltagare)

### Fas 2.3: Spells & Magi — "Eldens kraft"

Spell-systemet med spell slots, kända spells och cantrips.

- [ ] Domain: SpellSlot value object (Level, MaxSlots, CurrentSlots), lägg till på Character
- [ ] Domain: CombatRules — CastSpell (spell save DC = 8 + proficiency + spellcasting mod, spell attack = d20 + proficiency + spellcasting mod)
- [ ] Domain: SpellcastingAbility per CharacterClass (Wizard=INT, Cleric=WIS, Paladin=CHA, Ranger=WIS)
- [ ] Application: CastSpellCommand (characterId, spellId, targetId?) — validera känd spell, rätt klass, spell slot tillgänglig
- [ ] Application: LearnSpellCommand, GetKnownSpellsQuery
- [ ] Api: Utöka CombatController — POST cast-spell
- [ ] Api: SpellController — GET known-spells, POST learn
- [ ] Frontend: Spell-meny i combat (visa kända spells, grayed out om inga slots kvar)
- [ ] Frontend: Spell slot display i CharacterSidebar ("Level 1: 2/3")
- [ ] Seed data: 8-10 grundspells (Fire Bolt cantrip, Magic Missile, Cure Wounds, Shield, Healing Word, Guiding Bolt, Thunderwave, Bless)
- [ ] Seed data: Koppla spells till CharacterClass via RequiredClass

### Fas 2.4: Död, Death Saves & Permanent Död — "Ingen är säker"

> *GoT-tema: Döden är verklig och permanent. Ingen karaktär är immun.*

D&D 5e death saving throws. Permanenta konsekvenser.

- [ ] Domain: DeathSaveState på CombatParticipant (Successes 0-3, Failures 0-3, IsStabilized, IsDead)
- [ ] Domain: CombatRules — DeathSavingThrow (d20: 10+ = success, <10 = failure, nat 20 = regain 1 HP, nat 1 = 2 failures)
- [ ] Domain: Regel: 3 successes = stabiliserad, 3 failures = död. Damage vid 0 HP = automatic failure. Massive damage = instant death
- [ ] Domain: Character.IsDead property, Character.Kill() metod (permanent)
- [ ] Domain: CompanionDiedEvent (ny domain event — companions kan dö permanent)
- [ ] Application: DeathSavingThrowCommand (characterId) — automatisk vid 0 HP på spelarens tur
- [ ] Application: Combat defeat handling — CharacterDiedEvent vid spelardöd
- [ ] Frontend: Death save UI (visa 3 success/failure-cirklar)
- [ ] Frontend: "YOU DIED" overlay (val: ladda sparfil / ny karaktär)
- [ ] Frontend: Companion death i combat log med allvarlig ton ("Thorin has fallen. His eyes grow dim...")

### Fas 2.5: Monster AI — "Bestens instinkt"

RuleBasedCombatAI med alla AIStrategy-varianter.

- [ ] Infrastructure: RuleBasedCombatAI implementerar ICombatAI — strategy pattern
- [ ] Infrastructure: AggressiveStrategy (alltid attack, fokusera lägst HP)
- [ ] Infrastructure: DefensiveStrategy (Dodge om HP < 50%, annars attack starkaste)
- [ ] Infrastructure: TacticalStrategy (fokusera spellcasters, Disengage från melee)
- [ ] Infrastructure: CowardlyStrategy (Flee om HP < 30%, fokusera svagaste fienden)
- [ ] Infrastructure: BerserkerStrategy (alltid attack, ignorera egen HP, aldrig Dodge)
- [ ] Infrastructure: SupportiveStrategy (heal allierade om HP < 50%, annars attack)
- [ ] Application: Integrera ICombatAI i EndTurnCommand — monster-turer baseras på AIStrategy
- [ ] Application: FleeCombatCommand (characterId) — DEX check vs monster passive Perception
- [ ] Api: Utöka CombatController — POST flee
- [ ] Frontend: Monster-turens handlingar visas i combat log med korta fördröjningar
- [ ] Seed data: 3 nya monster — Skeleton (Aggressive), Giant Spider (Tactical), Orc Warrior (Berserker)

### Fas 2.6: Belöningar & Level Up — "Svärdets lön"

XP, guld, loot, level up.

- [ ] Domain: CombatRewardCalculator — XP från Monster.ExperienceReward, guld-drop baserat på CR
- [ ] Domain: Character.LevelUp() — öka Level, MaxHp (hit die per klass + CON mod), nya spell slots, raise LeveledUpEvent
- [ ] Domain: HitDie per CharacterClass (Fighter d10, Wizard d6, Rogue d8, Cleric d8, Ranger d10, Paladin d10)
- [ ] Application: ClaimCombatRewardsCommand (characterId) — dela ut XP/guld/loot efter Victory
- [ ] Application: LevelUpCommand (characterId) — kontrollera CanLevelUp(), utför LevelUp()
- [ ] Application: GetLevelUpInfoQuery (characterId) — visa vad som förbättras
- [ ] Api: Utöka CombatController — POST claim-rewards
- [ ] Api: CharacterController — POST level-up, GET level-up-info
- [ ] Frontend: Victory screen (XP gained, gold looted, items found)
- [ ] Frontend: Level up notification och modal (nya stats, HP-ökning, spell slots)
- [ ] Seed data: LootTable för varje monster (t.ex. Goblin droppar Short Sword 20%)
- [ ] Seed data: 5-8 nya items (Longsword, Chain Mail, Mana Potion, Antidote, Iron Dagger, Shortbow)

---

## Fas 3: Inventory & Equipment

### Fas 3.1: Inventory System — "Ryggsäckens hemligheter"

Grundläggande inventory: plocka upp, kasta, använd, visa items.

- [ ] Domain: Character.AddToInventory(Item, quantity), RemoveFromInventory(), UseConsumable() — metoder med validering
- [ ] Domain: InventorySlot.Create() factory, hantera stackable items (Consumable.IsStackable)
- [ ] Domain: Max inventory-storlek (20 slots initialt)
- [ ] Application: AddItemToInventoryCommand, RemoveItemFromInventoryCommand, UseItemCommand
- [ ] Application: GetInventoryQuery (characterId) — alla InventorySlots med Item-detaljer
- [ ] Api: InventoryController — GET inventory, POST add, POST remove, POST use
- [ ] Frontend: Inventory panel (grid-layout 4x5, item-ikoner med quantity badges)
- [ ] Frontend: Item tooltip on hover (namn, beskrivning, rarity-färg, stats, värde)
- [ ] Frontend: Kontextmeny på item (Use, Equip, Drop)
- [ ] Integrera med combat: rewards läggs automatiskt i inventory

### Fas 3.2: Equipment System — "Rustningens tyngd"

Utrustning med 11 slots, AC-beräkning, vapenanvändning i combat.

- [ ] Domain: Character.Equip(itemId, slot), Unequip(slot) — validera RequiredLevel, RequiredClass
- [ ] Domain: Character.RecalculateArmorClass() — base 10 + DEX mod + summa ArmorBonus
- [ ] Domain: Använd Equipment.DamageDice i combat istället för fast värde
- [ ] Application: EquipItemCommand, UnequipItemCommand, GetEquipmentQuery
- [ ] Api: EquipmentController — GET equipment, POST equip, POST unequip
- [ ] Frontend: Equipment paper-doll (11 slots: Head, Chest, Legs, Feet, Hands, MainHand, OffHand, Ring1, Ring2, Amulet, Back)
- [ ] Frontend: Drag-and-drop från inventory till equipment slot
- [ ] Frontend: Visa stat-förändringar vid equip ("+2 AC" i grönt/rött)
- [ ] Frontend: Uppdatera CharacterSidebar med equipped items
- [ ] Seed data: 10-15 nya items (Chain Mail, Greatsword, Longbow, Staff, Robe of Protection, Ring of Strength, etc.)

---

## Fas 4: Quests

### Fas 4.1: Quest Engine — "Uppdragens väg"

Grundläggande quest-system: acceptera, tracka objectives, lämna in.

- [ ] Domain: Quest.IsAvailableFor(Character) — kontrollera RequiredLevel, faction standing
- [ ] Domain: CharacterQuest.Accept(), AdvanceObjective(), Complete(), Fail() — state machine (Available → Active → Completed/Failed)
- [ ] Domain: Objektiv-progress tracking (t.ex. "killed 2/5 goblins")
- [ ] Application: AcceptQuestCommand, UpdateQuestProgressCommand, CompleteQuestCommand
- [ ] Application: GetQuestLogQuery (aktiva, avslutade, misslyckade)
- [ ] Application: GetAvailableQuestsQuery (characterId, npcId)
- [ ] Api: QuestController — GET available, GET log, POST accept, POST complete
- [ ] Frontend: Quest log panel (tabbar: Active / Completed / Failed, progress bars)
- [ ] Frontend: Quest tracker i HUD (aktiva objectives uppe till höger)
- [ ] Frontend: NPC quest-ikon (!) i dialogsystemet
- [ ] Seed data: 5 quests (Kill goblins, Collect herbs, Explore cave, Talk to Elder Mirael, Deliver sword)

### Fas 4.2: Moraliska val & konsekvenser — "Inget är svartvitt"

> *GoT-tema: Gråzons-beslut med verkliga, permanenta konsekvenser. Inga rätta svar.*

- [ ] Domain: QuestChoice entity (QuestId, ChoiceIndex, Description, ConsequenceType, ConsequenceData)
- [ ] Domain: QuestConsequenceType enum (ReputationChange, NpcDeath, NpcBetrayal, ItemLost, AlternateReward, FactionWar)
- [ ] Domain: NpcDiedEvent — NPCs kan dö permanent som konsekvens av spelarval
- [ ] Application: MakeQuestChoiceCommand — applicera konsekvenser (ModifyReputation, KillNpc, etc.)
- [ ] Frontend: Quest choice modal (visa alternativ med tvetydiga konsekvenser — spelaren ska ana, inte veta)
- [ ] Frontend: Consequence notification ("Elder Mirael's body is found by the river. The village mourns.")
- [ ] Frontend: Döda NPCs försvinner från kartan permanent
- [ ] Seed data: 2-3 quests med moraliskt gråa val (t.ex. "Banditerna har en familj. Dödar du dem, eller låter du dem gå?" — bägge val har konsekvenser)

---

## Fas 5: Economy & Shops

### Fas 5.1: Butiker & handel — "Guldets glans"

Köp- och säljsystem med NPC-butiker.

- [ ] Domain: Shop.GetBuyPrice(Item), Shop.GetSellPrice(Item) — med price modifiers
- [ ] Domain: Character.SpendGold(amount), EarnGold(amount) — med validering
- [ ] Domain: ShopInventoryEntry.Purchase(quantity) — minska stock
- [ ] Application: BuyItemCommand, SellItemCommand — validera guld, lagerplats, stock
- [ ] Application: GetShopInventoryQuery (shopId)
- [ ] Api: ShopController — GET inventory, POST buy, POST sell
- [ ] Frontend: Shop UI (två kolumner: Shop Inventory | Din Inventory, priser, Buy/Sell)
- [ ] Frontend: Guldvisning i shop header
- [ ] Frontend: Koppla IsMerchant-NPCs till shop via klick
- [ ] Seed data: 2 shops (Torvin's Smithy, Alchemist's Stall) med 10-15 items per shop

### Fas 5.2: Dynamisk ekonomi — "Marknadens nycker"

Prisanpassning baserat på faction standing.

- [ ] Domain: Shop.GetModifiedBuyPrice(Item, FactionStanding) — rabatt vid Friendly+, markup vid Unfriendly
- [ ] Domain: Prismodifikation per standing (Hostile: kan ej handla, Unfriendly: +25%, Neutral: 0%, Friendly: -10%, Honored: -20%, Revered: -30%)
- [ ] Application: Uppdatera Buy/Sell commands med faction-baserad prissättning
- [ ] Frontend: Visa original-pris och modifierat pris (överstruket + nytt i grönt/rött)
- [ ] Frontend: "Faction discount" badge i shop UI
- [ ] Domain: Om spelaren dödar en NPC-handlare försvinner butiken permanent (GoT-tema)

---

## Fas 6: Factions & Reputation

### Fas 6.1: Reputation System — "Makten och lojaliteten"

Factions med reputation-tiers och kaskadeffekter.

- [ ] Domain: FactionRelation entity (FactionId, RelatedFactionId, RelationType: Allied/Neutral/Rival/Hostile)
- [ ] Domain: ReputationRules — poängintervall per FactionStanding (Hostile <-3000, Unfriendly -3000 to -1, Neutral 0-2999, Friendly 3000-5999, Honored 6000-8999, Revered 9000+)
- [ ] Domain: CharacterFactionReputation.ModifyReputation(points) — uppdatera poäng och Standing automatiskt
- [ ] Domain: Kaskad-effekt: öka reputation med en faction → minska med deras rivaler (GoT: ingen allians är gratis)
- [ ] Application: ModifyReputationCommand med kaskad-beräkning
- [ ] Application: GetFactionStandingsQuery, GetFactionInfoQuery
- [ ] Api: FactionController — GET standings, GET faction-info
- [ ] Frontend: Faction standings panel (progress bar från Hostile till Revered)
- [ ] Frontend: Reputation change notification ("+150 Iron Shield", "-75 Shadow Court")
- [ ] Seed data: 5 factions med relationer (Iron Shield rivaljer Shadow Court, Arcane Circle rivaler Nature's Covenant, etc.)

### Fas 6.2: Politisk intrig — "När tronerna faller"

> *GoT-tema: Förråderier, maktskiften, hemliga allianser. Dina val ritar om världskartan.*

- [ ] Domain: FactionEvent entity (EventType, Description, AffectedFactionIds, TriggeredByQuestId?)
- [ ] Domain: FactionEventType enum (Alliance, War, Betrayal, Coup, Trade, Assassination)
- [ ] Domain: NPC.FactionLoyalty (0-100) — NPCs kan byta sida om lojaliteten är låg
- [ ] Application: TriggerFactionEventCommand — applicera world events baserat på spelarval
- [ ] Application: FactionEventHandlers — NPC byter faction, shop-priser ändras, nya quests öppnas/stängs
- [ ] Frontend: World Events notification ("The Shadow Court has betrayed the Merchant's League. Trade routes are disrupted.")
- [ ] Frontend: Faction-ikoner på NPCs ändras om de byter sida
- [ ] Seed data: 3-4 förlagrade faction events som triggas av quest choices

---

## Fas 7: Save System

### Fas 7.1: Sparning & laddning — "Skriv din saga"

Manuell save/load med save slots.

- [ ] Domain: SaveGame.Create() — serialisera all relevant state till JSON
- [ ] Domain: GameSettings value object (Difficulty: Normal/Hard/Ironman, PermadeathEnabled, AutosaveEnabled)
- [ ] Domain: Character.GameSettings — sätts vid character creation
- [ ] Application: SaveGameCommand (characterId, slotNumber, saveName) — serialisera allt (Character, Inventory, Quests, Reputations, World Events)
- [ ] Application: LoadGameCommand, GetSaveSlotsQuery, DeleteSaveCommand
- [ ] Api: SaveGameController — GET slots, POST save, POST load, DELETE save
- [ ] Frontend: Save/Load meny (overlay med save slots, preview: karaktärsnamn, level, zon, datum)
- [ ] Frontend: Quick save (Ctrl+S) och quick load (Ctrl+L)

### Fas 7.2: Autosave & Permadeath — "Öden är obeveklig"

> *GoT-tema: I Ironman mode finns det inga andra chanser. Ditt val är permanent.*

- [ ] Application: AutoSaveCommand — triggas vid zonbyte, combat victory, quest completion
- [ ] Application: AutoSave event handlers (lyssna på ZoneEnteredEvent, CombatEndedEvent, QuestCompletedEvent)
- [ ] Application: Ironman mode — en enda save-slot, skrivs över vid autosave, ingen manual save
- [ ] Application: Permadeath-hantering — om PermadeathEnabled och karaktären dör: radera save, permanent Game Over
- [ ] Frontend: Difficulty/permadeath val vid character creation (tydlig varning)
- [ ] Frontend: Autosave-indikator (liten ikon nere till höger)
- [ ] Frontend: Game Over screen för permadeath (gravsten med karaktärens statistik: tid, monster dödade, quests)
- [ ] Frontend: Ironman-badge i CharacterSidebar

---

## Fas 8: Professions & Crafting

### Fas 8.1: Insamling & professioner — "Lärlingsstiden"

Profession-system med skill levels och gathering.

- [ ] Domain: CharacterProfession.GainSkill(amount) — öka SkillLevel (max 300)
- [ ] Domain: Character.LearnProfession(professionId)
- [ ] Domain: ResourceNode entity (ZoneId, Position, ProfessionType, RequiredSkillLevel, ItemId, RespawnTime)
- [ ] Application: GatherResourceCommand — kontrollera profession, skill level, ge item + skill gain
- [ ] Application: GetProfessionsQuery, LearnProfessionCommand
- [ ] Api: ProfessionController — GET professions, POST learn, POST gather
- [ ] Frontend: Profession panel (8 professions, progress bar 1-300)
- [ ] Frontend: Resource node interaktion på kartan (klicka för att samla)
- [ ] Seed data: 8 professions, 10-15 CraftingMaterial items (Iron Ore, Silverleaf Herb, Linen Cloth, etc.)
- [ ] Seed data: ResourceNodes i zoner (malmådror i grotta, örter i skog)

### Fas 8.2: Crafting — "Skapandets konst"

Recipe-system: kombinera material till nya items.

- [ ] Domain: Recipe.CanCraft(CharacterProfession, Inventory) — validera skill level och ingredienser
- [ ] Domain: Recipe.Craft() — konsumera ingredienser, ge output, chans för skill gain
- [ ] Application: CraftItemCommand — validera, konsumera, skapa item, ge skill XP
- [ ] Application: GetRecipesQuery, GetRecipeDetailsQuery
- [ ] Api: CraftingController — GET recipes, GET recipe-details, POST craft
- [ ] Frontend: Crafting panel (välj profession → visa recept → ingredienser → Craft-knapp)
- [ ] Frontend: Recipe browser med filter (per profession, per skill level)
- [ ] Frontend: Ingredient-display (visa required items med checkmarks)
- [ ] Seed data: 15-20 recipes (Blacksmithing: Iron Dagger, Steel Sword; Alchemy: Health Potion, Antidote; Enchanting: Ring of Strength)

---

## Fas 9: Party & Companions

### Fas 9.1: Rekrytering & party-hantering — "Följeslagare i mörkret"

Companion-system: rekrytera NPCs, hantera gruppen, companions i combat.

- [ ] Domain: Companion.Recruit() factory, Dismiss(), SetActive(bool) — max 3 aktiva companions
- [ ] Domain: Companion.TakeDamage(), Heal() — liknande Character
- [ ] Domain: CompanionRecruitedEvent, CompanionDismissedEvent
- [ ] Application: RecruitCompanionCommand, DismissCompanionCommand, GetPartyQuery, SetCompanionActiveCommand
- [ ] Api: PartyController — GET party, POST recruit, POST dismiss, POST set-active
- [ ] Frontend: Party panel i CharacterSidebar (companion-porträtt, HP, klass)
- [ ] Frontend: Rekrytera companion via NPC dialogue ("Join me")
- [ ] Integrera i combat: companions som CombatParticipant, ICombatAI styr beteende

### Fas 9.2: Companion AI & lojalitet — "Förlåt, ingen litar på någon"

> *GoT-tema: Companions har egna agendor. Lojalitet måste förtjänas — och kan förloras.*

- [ ] Domain: Companion.Loyalty (0-100) — påverkas av spelarens handlingar, quest-val, faction standing
- [ ] Domain: Companion.Personality enum (Loyal, Ambitious, Cowardly, Vengeful, Mercenary)
- [ ] Domain: CompanionBetrayalRules — om Loyalty < 20: chans för förräderi (lämna party, stöld, attack)
- [ ] Domain: CompanionBetrayedEvent
- [ ] Application: ModifyCompanionLoyaltyCommand — triggas av quest val, faction changes, combat events
- [ ] Application: CompanionBetrayalHandler — förräderi-konsekvenser (companion attackerar, stjäl items, försvinner)
- [ ] Frontend: Loyalty-indikator (INTE exakt siffra: "Seems content" / "Appears uneasy" / "Looks resentful")
- [ ] Frontend: Förråder-cutscene ("You wake to find Lyra gone. Your gold pouch is lighter...")
- [ ] Seed data: 3-4 companions (Lyra the Rogue — Ambitious, Thorin the Fighter — Loyal, Sera the Cleric — Mercenary, Kael the Wizard — Vengeful)

### Fas 9.3: Permanent companion-död & Pets — "Sorg och trohet"

> *GoT-tema: Döden är permanent. Resurrection finns — men till ett högt pris.*

- [ ] Domain: Companion death saving throws (samma som spelare)
- [ ] Domain: ResurrectionRules — Revivify (Cleric 5+, 300 gp diamond), Raise Dead (Cleric 9+, 500 gp diamond)
- [ ] Domain: Pet entity — enklare än companions, ingen loyalty, combat participant
- [ ] Application: ResurrectCompanionCommand — validera spell, material, guld
- [ ] Application: SummonPetCommand, DismissPetCommand
- [ ] Api: Utöka PartyController — POST resurrect, POST summon-pet, POST dismiss-pet
- [ ] Frontend: "Memorial" panel (döda companions visas med grått, datum + plats de dog)
- [ ] Frontend: Sorg-narrativ vid companion-död ("The group is quieter tonight. Thorin's empty bedroll is a reminder...")
- [ ] Seed data: 2 pets (Wolf Pup, War Hawk)

---

## Fas 10: Världsbygge & Politiskt System

### Fas 10.1: Utökad värld — "Kontinenternas kall"

> *Warcraft-inspiration: En stor värld med distinkta regioner, faction-hubbar och äventyr i varje hörn.*

- [ ] Domain: Zone.Continent, Zone.RegionName — för gruppering
- [ ] Domain: WorldMapRegion value object (Continent, RegionName, DangerLevel 1-5)
- [ ] Application: GetWorldMapQuery — alla discovered zones grupperade per region
- [ ] Application: DiscoverZoneCommand (characterId, zoneId)
- [ ] Api: WorldController — GET world-map
- [ ] Frontend: World map overlay (klickbar karta, färger baserat på DangerLevel)
- [ ] Frontend: Fog of war — oupptäckta zoner gråa/otydliga
- [ ] Seed data: 6-8 nya zoner (Ironhold Fortress, Shadow Market, Arcane Tower, Sacred Grove, Merchant's Harbor, etc.)
- [ ] Seed data: ZoneConnections och tiles för alla nya zoner

### Fas 10.2: Dynamisk värld — "En värld som andas"

Tid, world events och vila.

- [ ] Domain: WorldState entity (CurrentDay, TimeOfDay: Dawn/Day/Dusk/Night, ActiveEvents)
- [ ] Domain: TimeOfDay-effekter (Night: encounter chance +50%, shops stängda, sämre sikt)
- [ ] Application: AdvanceTimeCommand — tid går framåt vid vila, zonbyten, combat
- [ ] Application: RestCommand (kort vila: 1 hour, lång vila: 8 hours — D&D 5e regler)
- [ ] Application: ProcessWorldEventsCommand — kontrollera aktiva world events
- [ ] Api: WorldController — GET world-state, POST rest
- [ ] Frontend: Tid-indikator i HUD (ikon + dag-räknare)
- [ ] Frontend: Visuell skillnad dag/natt (mörkare bakgrund, blått filter)
- [ ] Seed data: 3-4 world events (banditraid, epidemi, festival)

### Fas 10.3: Politiskt maktspel — "Spelet om tronen"

> *GoT-tema: Factions kämpar om makt. Dina handlingar ritar om den politiska kartan.*

- [ ] Domain: FactionPowerBalance (TerritoryCount, MilitaryStrength, EconomicPower, PoliticalInfluence)
- [ ] Domain: PoliticalAction enum (SupportFaction, SabotageFaction, FormAlliance, BreakAlliance, Assassinate, Negotiate)
- [ ] Application: TakePoliticalActionCommand — stor konsekvenshantering med ripple effects
- [ ] Application: GetPowerBalanceQuery — alla factions maktposition
- [ ] Application: FactionWarHandler — krig: pris-ökning, farligare zoner, nya quests
- [ ] Api: PoliticsController — GET power-balance, POST political-action
- [ ] Frontend: Politisk översikt (maktbalans-diagram, relations-karta)
- [ ] Frontend: "Whispers" system — rykten om kommande events ("Rumors say the Shadow Court is planning something...")
- [ ] Seed data: Initial power balance, 5-6 politiska scenarios

---

## Fas 11: Admin Backoffice

### Fas 11.1: Admin CRUD — "Skaparens verktyg"

Admin-gränssnitt för all game content.

- [ ] Api: Admin controllers (CRUD för Items, Monsters, Zones, NPCs, Quests, Factions, Spells, Shops, Recipes)
- [ ] Frontend: Admin layout under /admin (sidebar navigation)
- [ ] Frontend: Generisk DataTable komponent (sorterbar, filtrerbar, paginerad)
- [ ] Frontend: Generisk FormModal komponent (dynamiskt formulär per entity)
- [ ] Frontend: Admin dashboard (översikt: antal monsters, items, quests, zones, etc.)
- [ ] Frontend: Item-formulär med typ-val (Equipment/Consumable/CraftingMaterial visar rätt fält)

### Fas 11.2: Avancerade admin-verktyg — "Kartritarens bord"

Zone tile editor och avancerade verktyg.

- [ ] Frontend: Zone Tile Editor (klicka för att måla tiles, välj TileType, toggle walkable/interactable)
- [ ] Frontend: NPC-placering på tile map (drag-and-drop)
- [ ] Frontend: Monster spawn point editor
- [ ] Frontend: Resource node placement tool
- [ ] Frontend: Quest flow visualizer (objectives och choices som flödesschema)
- [ ] Frontend: Dialogue tree editor (visuell editor för NPC-dialogträd)
- [ ] Frontend: Loot table editor (drag items, sätt drop chance)
- [ ] Frontend: Data export/import (ladda ner/upp seed data JSON)

---

## Fas 12+: Polish & Expansion

### Fas 12.1: Fler klasser & races — "Hjältarnas mångfald"

- [ ] Domain: Utöka CharacterClass med Barbarian, Bard, Druid, Monk, Sorcerer, Warlock
- [ ] Domain: Klassspecifika features (Barbarian Rage, Bard Inspiration, Druid Wild Shape, Monk Ki, etc.)
- [ ] Domain: Underraser (High Elf, Wood Elf, Hill Dwarf, Mountain Dwarf, etc.)
- [ ] Domain: Racial abilities (Darkvision, Fey Ancestry, Dwarven Resilience, etc.)
- [ ] Seed data: Spells för alla nya caster-klasser
- [ ] Frontend: Uppdatera character creation med nya klasser/races

### Fas 12.2: Status Effects — "Förbannelsernas grepp"

- [ ] Domain: StatusEffect entity (Name, Duration, EffectType, Magnitude, IsDebuff)
- [ ] Domain: StatusEffectType enum (Poisoned, Stunned, Blessed, Frightened, Charmed, Paralyzed, Blinded, Invisible, Hasted, Slowed)
- [ ] Domain: CombatRules — applicera status effects från spells och monster abilities
- [ ] Domain: Duration countdown och saving throws för att bryta
- [ ] Application: ApplyStatusEffectCommand, RemoveStatusEffectCommand
- [ ] Frontend: Status effect ikoner på combat participants (tooltip: duration, effekt)

### Fas 12.3: Avancerad AI — "Den tänkande fienden"

- [ ] Infrastructure: Behavior Tree-baserad AI (ersätter strategy pattern för komplexa fiender)
- [ ] Infrastructure: Utility AI för bossar (väg alternativ baserat på situation)
- [ ] Domain: BossMonster subclass med speciella abilities och phases
- [ ] Seed data: 3-5 boss monsters (Goblin King, Dark Wizard, Ancient Spider Queen, Orc Warlord)
- [ ] Frontend: Boss HP bar (stor, centrerad, med phase indicators)

### Fas 12.4: Ljud & animation — "Världen får liv"

- [ ] Frontend: Ambient ljud per zon (skog, stad, grotta, strid)
- [ ] Frontend: Stridljud (sword clash, spell cast, hit, miss, death)
- [ ] Frontend: Combat animationer (attack slash, spell effects, damage numbers)
- [ ] Frontend: Screen shake vid kritiska träffar
- [ ] Frontend: Musikaliskt tema per zon / combat

### Fas 12.5: Multiclass & avancerade builds — "Maktens pris"

- [ ] Domain: Multiclass-system (karaktär kan välja ny klass vid level up)
- [ ] Domain: Multiclass-regler (ability score prerequisites, spell slot merging)
- [ ] Domain: Feat-system (feats istället för ability score increase vid level 4, 8, 12, 16, 19)
- [ ] Seed data: 15-20 feats (Great Weapon Master, Sharpshooter, War Caster, Lucky, Tough, etc.)
- [ ] Frontend: Level up UI med multiclass- och feat-val
