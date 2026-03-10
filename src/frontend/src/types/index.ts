export interface SpellSlotDto {
  level: number;
  maxSlots: number;
  currentSlots: number;
}

export interface Character {
  id: string;
  name: string;
  race: Race;
  characterClass: CharacterClass;
  level: number;
  experiencePoints: number;
  currentHitPoints: number;
  maxHitPoints: number;
  armorClass: number;
  strength: number;
  dexterity: number;
  constitution: number;
  intelligence: number;
  wisdom: number;
  charisma: number;
  gold: number;
  currentZoneId: string;
  positionX: number;
  positionY: number;
  spellSlots: SpellSlotDto[] | null;
}

export interface ZoneDto {
  id: string;
  name: string;
  description: string;
  zoneType: number;
  width: number;
  height: number;
  worldMapX: number;
  worldMapY: number;
  isDiscovered: boolean;
}

export interface TileDto {
  id: string;
  zoneId: string;
  x: number;
  y: number;
  tileType: TileType;
  isWalkable: boolean;
  isInteractable: boolean;
  interactionData: string | null;
  tilesetSpriteName: string;
}

export interface NpcDto {
  id: string;
  name: string;
  description: string;
  positionX: number;
  positionY: number;
  isQuestGiver: boolean;
  isMerchant: boolean;
}

export interface GameStateDto {
  character: Character;
  currentZone: ZoneDto;
  tiles: TileDto[];
  npcsInZone: NpcDto[];
}

export interface MoveResultDto {
  success: boolean;
  message: string | null;
  gameState: GameStateDto;
  combatEncounter: CombatStateDto | null;
}

export enum CombatActionType {
  Attack = 0,
  CastSpell = 1,
  Dodge = 2,
  Dash = 3,
  UseItem = 4,
  Disengage = 5,
  Help = 6,
  Hide = 7,
}

export enum CombatCondition {
  Dodging = 0,
  Disengaging = 1,
  Hidden = 2,
  Helped = 3,
  HasAdvantage = 4,
  HasDisadvantage = 5,
}

export interface CombatParticipantDto {
  id: string;
  name: string;
  participantType: number;
  currentHp: number;
  maxHp: number;
  armorClass: number;
  initiativeRoll: number;
  isAlive: boolean;
  activeConditions: number[];
}

export interface CombatStateDto {
  encounterId: string;
  currentRound: number;
  activeParticipantId: string;
  isPlayerTurn: boolean;
  isComplete: boolean;
  hasTakenAction: boolean;
  outcome: number | null;
  participants: CombatParticipantDto[];
  combatLog: string[];
}

export interface CombatActionResultDto {
  actionType: number;
  actorName: string;
  targetName: string | null;
  naturalRoll: number | null;
  totalRoll: number | null;
  targetAC: number | null;
  isHit: boolean;
  isCritical: boolean;
  damageDealt: number;
  healingDone: number;
  description: string;
  updatedState: CombatStateDto;
}

export interface CombatConsumableDto {
  itemId: string;
  name: string;
  effectType: string;
  effectValue: number;
  quantity: number;
}

export interface CombatSpellDto {
  spellId: string;
  name: string;
  spellLevel: number;
  damageDice: string | null;
  healingDice: string | null;
  isCantrip: boolean;
  canCast: boolean;
  targetType: 'enemy' | 'ally' | 'self' | 'none';
}

export interface CombatSpellInfoDto {
  spells: CombatSpellDto[];
  spellSlots: SpellSlotDto[];
}

export interface KnownSpellDto {
  spellId: string;
  name: string;
  description: string;
  spellLevel: number;
  school: number;
  damageDice: string | null;
  damageType: number | null;
  healingDice: string | null;
  isCantrip: boolean;
  canCast: boolean;
}

export interface CharacterSpellInfoDto {
  knownSpells: KnownSpellDto[];
  spellSlots: SpellSlotDto[];
  spellSaveDC: number | null;
  spellAttackBonus: number | null;
}

export interface DialogueNodeDto {
  nodeId: string;
  text: string;
  speakerName: string;
  options: DialogueOptionDto[];
  isEnd: boolean;
}

export interface DialogueOptionDto {
  index: number;
  text: string;
}

export interface CreateCharacterRequest {
  name: string;
  race: Race;
  characterClass: CharacterClass;
  strength: number;
  dexterity: number;
  constitution: number;
  intelligence: number;
  wisdom: number;
  charisma: number;
}

export enum Race {
  Human = 0,
  Elf = 1,
  Dwarf = 2,
  Halfling = 3,
  HalfElf = 4,
  HalfOrc = 5,
  Gnome = 6,
  Dragonborn = 7,
  Tiefling = 8,
}

export enum CharacterClass {
  Fighter = 0,
  Wizard = 1,
  Rogue = 2,
  Cleric = 3,
  Ranger = 4,
  Paladin = 5,
}

export enum TileType {
  Grass = 0,
  Stone = 1,
  Water = 2,
  Wall = 3,
  Door = 4,
  Sand = 5,
  Dirt = 6,
  Wood = 7,
  Bridge = 8,
  StairsUp = 9,
  StairsDown = 10,
  Encounter = 11,
}

export enum ItemRarity {
  Common = 0,
  Uncommon = 1,
  Rare = 2,
  VeryRare = 3,
  Legendary = 4,
}

export enum EquipmentSlotType {
  Head = 0,
  Chest = 1,
  Legs = 2,
  Feet = 3,
  Hands = 4,
  MainHand = 5,
  OffHand = 6,
  Ring1 = 7,
  Ring2 = 8,
  Amulet = 9,
  Back = 10,
}

export const RaceLabels: Record<Race, string> = {
  [Race.Human]: 'Human',
  [Race.Elf]: 'Elf',
  [Race.Dwarf]: 'Dwarf',
  [Race.Halfling]: 'Halfling',
  [Race.HalfElf]: 'Half-Elf',
  [Race.HalfOrc]: 'Half-Orc',
  [Race.Gnome]: 'Gnome',
  [Race.Dragonborn]: 'Dragonborn',
  [Race.Tiefling]: 'Tiefling',
};

export const ClassLabels: Record<CharacterClass, string> = {
  [CharacterClass.Fighter]: 'Fighter',
  [CharacterClass.Wizard]: 'Wizard',
  [CharacterClass.Rogue]: 'Rogue',
  [CharacterClass.Cleric]: 'Cleric',
  [CharacterClass.Ranger]: 'Ranger',
  [CharacterClass.Paladin]: 'Paladin',
};
