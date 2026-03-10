import { useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import { executeCombatAction, endTurn, useItemInCombat } from '../../api/combatApi';
import { getGameState } from '../../api/gameApi';
import { useGameStore } from '../../store/gameStore';
import { useCombatStore } from '../../store/combatStore';
import { CombatActionType } from '../../types';
import { TurnOrderBar } from './TurnOrderBar';
import { CombatParticipantCard } from './CombatParticipantCard';
import { CombatLogPanel } from './CombatLogPanel';
import { CombatResultOverlay } from './CombatResultOverlay';
import { CombatActionMenu, getActionTargetType } from './CombatActionMenu';
import { ItemPickerPanel } from './ItemPickerPanel';

export function CombatView() {
  const navigate = useNavigate();
  const character = useGameStore((s) => s.character);
  const setIsInCombat = useGameStore((s) => s.setIsInCombat);
  const setGameState = useGameStore((s) => s.setGameState);
  const addNarrativeEvent = useGameStore((s) => s.addNarrativeEvent);

  const encounter = useCombatStore((s) => s.encounter);
  const selectedTargetId = useCombatStore((s) => s.selectedTargetId);
  const selectedAction = useCombatStore((s) => s.selectedAction);
  const isProcessing = useCombatStore((s) => s.isProcessing);
  const showItemPicker = useCombatStore((s) => s.showItemPicker);
  const setEncounter = useCombatStore((s) => s.setEncounter);
  const setSelectedTargetId = useCombatStore((s) => s.setSelectedTargetId);
  const setSelectedAction = useCombatStore((s) => s.setSelectedAction);
  const setIsProcessing = useCombatStore((s) => s.setIsProcessing);
  const setShowItemPicker = useCombatStore((s) => s.setShowItemPicker);
  const clearCombat = useCombatStore((s) => s.clearCombat);

  const handleSelectAction = useCallback((action: CombatActionType) => {
    if (!character || !encounter || isProcessing) return;

    const targetType = getActionTargetType(action);

    if (action === CombatActionType.UseItem) {
      setShowItemPicker(true);
      setSelectedAction(null);
      setSelectedTargetId(null);
      return;
    }

    setShowItemPicker(false);

    if (targetType) {
      // Needs target selection — set action and wait for click
      setSelectedAction(action);
      setSelectedTargetId(null);
      return;
    }

    // No target needed — execute immediately
    executeAction(action);
  }, [character, encounter, isProcessing]);

  const executeAction = useCallback(async (action: CombatActionType, targetId?: string) => {
    if (!character || !encounter || isProcessing) return;

    setIsProcessing(true);
    setShowItemPicker(false);
    try {
      const { data } = await executeCombatAction(character.id, action, targetId);
      setEncounter(data.updatedState);
      setSelectedAction(null);
      setSelectedTargetId(null);
    } catch {
      addNarrativeEvent('Action failed.');
    } finally {
      setIsProcessing(false);
    }
  }, [character, encounter, isProcessing, setEncounter, setSelectedAction, setSelectedTargetId, setIsProcessing, addNarrativeEvent]);

  const handleTargetSelect = useCallback((targetId: string) => {
    if (selectedAction === null || isProcessing) return;
    executeAction(selectedAction as CombatActionType, targetId);
  }, [selectedAction, isProcessing, executeAction]);

  const handleEndTurn = useCallback(async () => {
    if (!character || !encounter || isProcessing) return;

    setIsProcessing(true);
    try {
      const { data } = await endTurn(character.id);
      setEncounter(data);
    } catch {
      addNarrativeEvent('End turn failed.');
    } finally {
      setIsProcessing(false);
    }
  }, [character, encounter, isProcessing, setEncounter, setIsProcessing, addNarrativeEvent]);

  const handleUseItem = useCallback(async (itemId: string) => {
    if (!character || !encounter || isProcessing) return;

    setIsProcessing(true);
    setShowItemPicker(false);
    try {
      const { data } = await useItemInCombat(character.id, itemId);
      setEncounter(data.updatedState);
    } catch {
      addNarrativeEvent('Failed to use item.');
    } finally {
      setIsProcessing(false);
    }
  }, [character, encounter, isProcessing, setEncounter, setIsProcessing, setShowItemPicker, addNarrativeEvent]);

  const handleContinue = useCallback(async () => {
    if (!character || !encounter) return;

    const isVictory = encounter.outcome === 0;

    clearCombat();
    setIsInCombat(false);

    if (isVictory) {
      try {
        const { data } = await getGameState(character.id);
        setGameState(data);
        addNarrativeEvent('The battle is over. You are victorious!');
      } catch {
        addNarrativeEvent('Combat ended.');
      }
    } else {
      addNarrativeEvent('You have fallen in battle...');
      navigate('/');
    }
  }, [character, encounter, clearCombat, setIsInCombat, setGameState, addNarrativeEvent, navigate]);

  if (!encounter) return null;

  const players = encounter.participants.filter((p) => p.participantType !== 2);
  const enemies = encounter.participants.filter((p) => p.participantType === 2);
  const targetType = selectedAction !== null ? getActionTargetType(selectedAction as CombatActionType) : null;

  const getStatusText = () => {
    if (isProcessing) return 'Processing...';
    if (!encounter.isPlayerTurn) return 'Waiting for enemies...';
    if (encounter.hasTakenAction) return 'Click End Turn to continue.';
    if (selectedAction === CombatActionType.Attack) return 'Select an enemy to attack.';
    if (selectedAction === CombatActionType.Help) return 'Select an ally to help.';
    return 'Choose an action.';
  };

  return (
    <div className="fixed inset-0 z-50 bg-black/80 flex items-center justify-center p-4">
      <div className="relative w-full max-w-4xl bg-gray-800 rounded-xl border border-gray-600 shadow-2xl flex flex-col max-h-[90vh] overflow-hidden">

        <div className="p-3 border-b border-gray-700">
          <div className="flex items-center justify-between mb-2">
            <h2 className="text-lg font-bold text-red-400">COMBAT</h2>
            <span className="text-xs text-gray-500">Round {encounter.currentRound}</span>
          </div>
          <TurnOrderBar
            participants={encounter.participants}
            activeParticipantId={encounter.activeParticipantId}
          />
        </div>

        <div className="flex-1 p-4 flex items-center justify-center gap-8 min-h-[200px]">
          <div className="flex flex-col gap-3 items-center">
            <p className="text-xs text-blue-400 font-bold uppercase tracking-wider">Party</p>
            {players.map((p) => (
              <CombatParticipantCard
                key={p.id}
                participant={p}
                isActive={p.id === encounter.activeParticipantId}
                isSelected={p.id === selectedTargetId}
                onClick={targetType === 'ally' && p.isAlive && encounter.isPlayerTurn && !encounter.isComplete
                  ? () => handleTargetSelect(p.id)
                  : undefined}
              />
            ))}
          </div>

          <div className="text-3xl font-bold text-gray-600">VS</div>

          <div className="flex flex-col gap-3 items-center">
            <p className="text-xs text-red-400 font-bold uppercase tracking-wider">Enemies</p>
            {enemies.map((e) => (
              <CombatParticipantCard
                key={e.id}
                participant={e}
                isActive={e.id === encounter.activeParticipantId}
                isSelected={e.id === selectedTargetId}
                onClick={targetType === 'enemy' && e.isAlive && encounter.isPlayerTurn && !encounter.isComplete
                  ? () => handleTargetSelect(e.id)
                  : undefined}
              />
            ))}
          </div>
        </div>

        <div className="border-t border-gray-700">
          <div className="p-3">
            <CombatLogPanel log={encounter.combatLog} />
          </div>

          {!encounter.isComplete && (
            <div className="px-4 pb-4">
              <div className="relative flex items-center gap-4">
                <CombatActionMenu
                  isPlayerTurn={encounter.isPlayerTurn}
                  hasTakenAction={encounter.hasTakenAction}
                  isComplete={encounter.isComplete}
                  isProcessing={isProcessing}
                  selectedAction={selectedAction}
                  onSelectAction={handleSelectAction}
                />

                {encounter.hasTakenAction && encounter.isPlayerTurn && (
                  <button
                    onClick={handleEndTurn}
                    disabled={isProcessing}
                    className="px-6 py-2 bg-yellow-600 hover:bg-yellow-500 disabled:opacity-40 disabled:cursor-not-allowed text-white font-bold rounded-lg transition-colors"
                  >
                    {isProcessing ? 'Processing...' : 'End Turn'}
                  </button>
                )}

                {showItemPicker && character && (
                  <ItemPickerPanel
                    characterId={character.id}
                    onUseItem={handleUseItem}
                    onClose={() => setShowItemPicker(false)}
                  />
                )}
              </div>
              <p className="text-sm text-gray-400 mt-2">{getStatusText()}</p>
            </div>
          )}
        </div>

        {encounter.isComplete && encounter.outcome !== null && (
          <CombatResultOverlay outcome={encounter.outcome} onContinue={handleContinue} />
        )}
      </div>
    </div>
  );
}
