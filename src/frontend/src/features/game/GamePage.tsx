import { useEffect, useState, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import { getGameState } from '../../api/gameApi';
import { getNpcDialogue } from '../../api/npcApi';
import { useGameStore } from '../../store/gameStore';
import { useMovement } from '../../hooks/useMovement';
import { CharacterSidebar } from './CharacterSidebar';
import { NarrativePanel } from './NarrativePanel';
import { TileMap } from '../map/TileMap';
import { DialogueModal } from '../npc/DialogueModal';
import { HowToPlayModal } from '../../components/HowToPlayModal';
import { CombatView } from '../combat/CombatView';
import type { NpcDto } from '../../types';

export function GamePage() {
  const navigate = useNavigate();
  const character = useGameStore((s) => s.character);
  const isInCombat = useGameStore((s) => s.isInCombat);
  const currentZone = useGameStore((s) => s.currentZone);
  const setGameState = useGameStore((s) => s.setGameState);
  const addNarrativeEvent = useGameStore((s) => s.addNarrativeEvent);
  const setActiveDialogue = useGameStore((s) => s.setActiveDialogue);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [showHelp, setShowHelp] = useState(false);

  const { move } = useMovement();

  useEffect(() => {
    if (!character) {
      navigate('/');
      return;
    }

    if (currentZone) return;

    setLoading(true);
    getGameState(character.id)
      .then(({ data }) => {
        setGameState(data);
        addNarrativeEvent(`You arrived in ${data.currentZone.name}.`);
      })
      .catch(() => setError('Failed to load game state.'))
      .finally(() => setLoading(false));
  }, [character, currentZone, navigate, setGameState, addNarrativeEvent]);

  const handleTileClick = useCallback((dx: number, dy: number) => {
    move(dx, dy);
  }, [move]);

  const handleNpcClick = useCallback(async (npc: NpcDto) => {
    if (!character) return;

    const dist = Math.abs(npc.positionX - character.positionX) + Math.abs(npc.positionY - character.positionY);
    if (dist > 1) {
      addNarrativeEvent(`You are too far away to speak with ${npc.name}.`);
      return;
    }

    try {
      const { data } = await getNpcDialogue(npc.id);
      setActiveDialogue({ npcId: npc.id, npcName: npc.name, node: data });
    } catch {
      addNarrativeEvent(`${npc.name} does not respond.`);
    }
  }, [character, addNarrativeEvent, setActiveDialogue]);

  if (!character) return null;

  if (loading) {
    return (
      <div className="min-h-screen bg-gray-900 text-white flex items-center justify-center">
        <p className="text-xl text-gray-400">Loading...</p>
      </div>
    );
  }

  if (error) {
    return (
      <div className="min-h-screen bg-gray-900 text-white flex items-center justify-center">
        <div className="text-center">
          <p className="text-red-400 mb-4">{error}</p>
          <button
            onClick={() => navigate('/')}
            className="px-4 py-2 bg-gray-700 hover:bg-gray-600 rounded-lg transition-colors"
          >
            Back to Menu
          </button>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-900 text-white">
      <div className="h-screen grid grid-cols-[280px_1fr_320px] gap-2 p-2">
        <div className="overflow-y-auto">
          <CharacterSidebar />
        </div>

        <div className="flex flex-col items-center justify-center relative">
          <button
            onClick={() => setShowHelp(true)}
            className="absolute top-2 right-2 w-8 h-8 bg-gray-700 hover:bg-gray-600 rounded-full text-gray-400 hover:text-amber-400 font-bold text-sm transition-colors z-10"
            title="How to Play"
          >
            ?
          </button>
          {currentZone && (
            <div className="w-full max-w-xl">
              <TileMap
                width={currentZone.width}
                height={currentZone.height}
                onTileClick={handleTileClick}
                onNpcClick={handleNpcClick}
              />
            </div>
          )}
        </div>

        <div className="overflow-y-auto">
          <NarrativePanel />
        </div>
      </div>

      <DialogueModal />
      {showHelp && <HowToPlayModal onClose={() => setShowHelp(false)} />}
      {isInCombat && <CombatView />}
    </div>
  );
}
