import type { CombatParticipantDto } from '../../types';

interface TurnOrderBarProps {
  participants: CombatParticipantDto[];
  activeParticipantId: string;
}

export function TurnOrderBar({ participants, activeParticipantId }: TurnOrderBarProps) {
  return (
    <div className="flex items-center gap-2 px-4 py-2 bg-gray-800 rounded-lg overflow-x-auto">
      <span className="text-xs text-gray-500 shrink-0">Initiative:</span>
      {participants.map((p) => {
        const isActive = p.id === activeParticipantId;
        const isEnemy = p.participantType === 2;
        const isDead = !p.isAlive;

        return (
          <div
            key={p.id}
            className={`flex items-center gap-1.5 px-2.5 py-1 rounded-full text-xs font-medium shrink-0 transition-all
              ${isDead ? 'bg-gray-700 text-gray-500 line-through' : ''}
              ${!isDead && isActive ? 'ring-2 ring-amber-400 shadow-lg shadow-amber-400/20' : ''}
              ${!isDead && isEnemy ? 'bg-red-900/60 text-red-300' : ''}
              ${!isDead && !isEnemy ? 'bg-blue-900/60 text-blue-300' : ''}
            `}
          >
            <span>{p.name}</span>
            <span className="text-gray-500">({p.initiativeRoll})</span>
          </div>
        );
      })}
    </div>
  );
}
