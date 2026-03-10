import { useState, useCallback, useEffect } from 'react';
import { moveCharacter } from '../api/movementApi';
import { useGameStore } from '../store/gameStore';
import { useCombatStore } from '../store/combatStore';

export function useMovement() {
  const character = useGameStore((s) => s.character);
  const isInCombat = useGameStore((s) => s.isInCombat);
  const setGameState = useGameStore((s) => s.setGameState);
  const setIsInCombat = useGameStore((s) => s.setIsInCombat);
  const addNarrativeEvent = useGameStore((s) => s.addNarrativeEvent);
  const activeDialogue = useGameStore((s) => s.activeDialogue);
  const setEncounter = useCombatStore((s) => s.setEncounter);
  const [isMoving, setIsMoving] = useState(false);

  const move = useCallback(async (dx: number, dy: number) => {
    if (!character || isMoving || activeDialogue || isInCombat) return;
    setIsMoving(true);
    try {
      const { data } = await moveCharacter(character.id, dx, dy);
      setGameState(data.gameState);
      if (data.message) addNarrativeEvent(data.message);

      if (data.combatEncounter) {
        addNarrativeEvent('You are ambushed!');
        setEncounter(data.combatEncounter);
        setIsInCombat(true);
      }
    } catch {
      addNarrativeEvent('Failed to move.');
    } finally {
      setIsMoving(false);
    }
  }, [character, isMoving, activeDialogue, isInCombat, setGameState, setIsInCombat, addNarrativeEvent, setEncounter]);

  useEffect(() => {
    const handler = (e: KeyboardEvent) => {
      if (e.target instanceof HTMLInputElement || e.target instanceof HTMLTextAreaElement) return;

      const keyMap: Record<string, [number, number]> = {
        ArrowUp: [0, -1], w: [0, -1], W: [0, -1],
        ArrowDown: [0, 1], s: [0, 1], S: [0, 1],
        ArrowLeft: [-1, 0], a: [-1, 0], A: [-1, 0],
        ArrowRight: [1, 0], d: [1, 0], D: [1, 0],
      };

      const dir = keyMap[e.key];
      if (dir) {
        e.preventDefault();
        move(dir[0], dir[1]);
      }
    };

    window.addEventListener('keydown', handler);
    return () => window.removeEventListener('keydown', handler);
  }, [move]);

  return { move, isMoving };
}
