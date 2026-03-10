import { useEffect, useRef } from 'react';
import { useGameStore } from '../../store/gameStore';

export function NarrativePanel() {
  const currentZone = useGameStore((s) => s.currentZone);
  const character = useGameStore((s) => s.character);
  const npcsInZone = useGameStore((s) => s.npcsInZone);
  const narrativeLog = useGameStore((s) => s.narrativeLog);
  const logEndRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    logEndRef.current?.scrollIntoView({ behavior: 'smooth' });
  }, [narrativeLog.length]);

  if (!currentZone || !character) return null;

  return (
    <div className="bg-gray-800 rounded-lg p-4 space-y-4 h-full flex flex-col overflow-hidden">
      <div className="shrink-0">
        <h2 className="text-lg font-bold text-amber-500">{currentZone.name}</h2>
        <p className="text-sm text-gray-300 leading-relaxed">{currentZone.description}</p>
      </div>

      <div className="border-t border-gray-700 pt-3 shrink-0">
        <p className="text-sm text-gray-400">
          You are standing at position ({character.positionX}, {character.positionY}).
        </p>
      </div>

      {npcsInZone.length > 0 && (
        <div className="border-t border-gray-700 pt-3 shrink-0">
          <h3 className="text-sm font-semibold text-gray-300 mb-2">People Nearby</h3>
          <ul className="space-y-2">
            {npcsInZone.map((npc) => (
              <li key={npc.id} className="bg-gray-700 rounded p-2">
                <div className="flex items-center gap-2">
                  <span className="text-amber-400 font-semibold text-sm">{npc.name}</span>
                  {npc.isQuestGiver && (
                    <span className="text-xs bg-yellow-800 text-yellow-300 px-1.5 rounded">Quest</span>
                  )}
                  {npc.isMerchant && (
                    <span className="text-xs bg-blue-800 text-blue-300 px-1.5 rounded">Shop</span>
                  )}
                </div>
                <p className="text-xs text-gray-400 mt-1">{npc.description}</p>
              </li>
            ))}
          </ul>
        </div>
      )}

      {narrativeLog.length > 0 && (
        <div className="border-t border-gray-700 pt-3 flex-1 min-h-0 flex flex-col">
          <h3 className="text-sm font-semibold text-gray-300 mb-2 shrink-0">Events</h3>
          <div className="flex-1 overflow-y-auto space-y-1">
            {narrativeLog.map((msg, i) => (
              <p key={i} className="text-xs text-gray-400 italic">{msg}</p>
            ))}
            <div ref={logEndRef} />
          </div>
        </div>
      )}
    </div>
  );
}
