import { create } from 'zustand';
import type { CombatStateDto, CombatActionResultDto } from '../types';

interface CombatStoreState {
  encounter: CombatStateDto | null;
  lastActionResult: CombatActionResultDto | null;
  selectedTargetId: string | null;
  selectedAction: number | null;
  isProcessing: boolean;
  showItemPicker: boolean;
  showSpellPicker: boolean;
  pendingSpellId: string | null;
  setEncounter: (encounter: CombatStateDto | null) => void;
  setLastActionResult: (result: CombatActionResultDto | null) => void;
  setSelectedTargetId: (id: string | null) => void;
  setSelectedAction: (action: number | null) => void;
  setIsProcessing: (processing: boolean) => void;
  setShowItemPicker: (show: boolean) => void;
  setShowSpellPicker: (show: boolean) => void;
  setPendingSpellId: (id: string | null) => void;
  clearCombat: () => void;
}

export const useCombatStore = create<CombatStoreState>((set) => ({
  encounter: null,
  lastActionResult: null,
  selectedTargetId: null,
  selectedAction: null,
  isProcessing: false,
  showItemPicker: false,
  showSpellPicker: false,
  pendingSpellId: null,
  setEncounter: (encounter) => set({ encounter }),
  setLastActionResult: (result) => set({ lastActionResult: result }),
  setSelectedTargetId: (id) => set({ selectedTargetId: id }),
  setSelectedAction: (action) => set({ selectedAction: action }),
  setIsProcessing: (processing) => set({ isProcessing: processing }),
  setShowItemPicker: (show) => set({ showItemPicker: show }),
  setShowSpellPicker: (show) => set({ showSpellPicker: show }),
  setPendingSpellId: (id) => set({ pendingSpellId: id }),
  clearCombat: () => set({
    encounter: null,
    lastActionResult: null,
    selectedTargetId: null,
    selectedAction: null,
    isProcessing: false,
    showItemPicker: false,
    showSpellPicker: false,
    pendingSpellId: null,
  }),
}));
