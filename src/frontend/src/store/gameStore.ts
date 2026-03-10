import { create } from 'zustand';
import type { Character, ZoneDto, TileDto, NpcDto, GameStateDto, DialogueNodeDto } from '../types';

interface ActiveDialogue {
  npcId: string;
  npcName: string;
  node: DialogueNodeDto;
}

interface GameState {
  character: Character | null;
  isInCombat: boolean;
  currentZone: ZoneDto | null;
  tiles: TileDto[];
  npcsInZone: NpcDto[];
  narrativeLog: string[];
  activeDialogue: ActiveDialogue | null;
  setCharacter: (character: Character) => void;
  setIsInCombat: (isInCombat: boolean) => void;
  setGameState: (state: GameStateDto) => void;
  clearGameState: () => void;
  addNarrativeEvent: (message: string) => void;
  setActiveDialogue: (dialogue: ActiveDialogue | null) => void;
}

export const useGameStore = create<GameState>((set) => ({
  character: null,
  isInCombat: false,
  currentZone: null,
  tiles: [],
  npcsInZone: [],
  narrativeLog: [],
  activeDialogue: null,
  setCharacter: (character) => set({ character, currentZone: null, tiles: [], npcsInZone: [] }),
  setIsInCombat: (isInCombat) => set({ isInCombat }),
  setGameState: (state) => set({
    character: state.character,
    currentZone: state.currentZone,
    tiles: state.tiles,
    npcsInZone: state.npcsInZone,
  }),
  clearGameState: () => set({
    character: null,
    isInCombat: false,
    currentZone: null,
    tiles: [],
    npcsInZone: [],
    narrativeLog: [],
    activeDialogue: null,
  }),
  addNarrativeEvent: (message) => set((state) => ({
    narrativeLog: [...state.narrativeLog, message].slice(-50),
  })),
  setActiveDialogue: (dialogue) => set({ activeDialogue: dialogue }),
}));
