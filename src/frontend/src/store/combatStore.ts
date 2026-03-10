import { create } from 'zustand';
import type { CombatStateDto, CombatActionResultDto } from '../types';

interface CombatStoreState {
  encounter: CombatStateDto | null;
  lastActionResult: CombatActionResultDto | null;
  selectedTargetId: string | null;
  selectedAction: number | null;
  isProcessing: boolean;
  showItemPicker: boolean;
  setEncounter: (encounter: CombatStateDto | null) => void;
  setLastActionResult: (result: CombatActionResultDto | null) => void;
  setSelectedTargetId: (id: string | null) => void;
  setSelectedAction: (action: number | null) => void;
  setIsProcessing: (processing: boolean) => void;
  setShowItemPicker: (show: boolean) => void;
  clearCombat: () => void;
}

export const useCombatStore = create<CombatStoreState>((set) => ({
  encounter: null,
  lastActionResult: null,
  selectedTargetId: null,
  selectedAction: null,
  isProcessing: false,
  showItemPicker: false,
  setEncounter: (encounter) => set({ encounter }),
  setLastActionResult: (result) => set({ lastActionResult: result }),
  setSelectedTargetId: (id) => set({ selectedTargetId: id }),
  setSelectedAction: (action) => set({ selectedAction: action }),
  setIsProcessing: (processing) => set({ isProcessing: processing }),
  setShowItemPicker: (show) => set({ showItemPicker: show }),
  clearCombat: () => set({
    encounter: null,
    lastActionResult: null,
    selectedTargetId: null,
    selectedAction: null,
    isProcessing: false,
    showItemPicker: false,
  }),
}));
