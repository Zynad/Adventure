import { HpBar } from '../../components/HpBar';
import { CombatCondition } from '../../types';
import type { CombatParticipantDto } from '../../types';

interface CombatParticipantCardProps {
  participant: CombatParticipantDto;
  isActive: boolean;
  isSelected: boolean;
  onClick?: () => void;
}

const conditionLabels: Record<number, { label: string; color: string }> = {
  [CombatCondition.Dodging]: { label: 'Dodging', color: 'bg-blue-600' },
  [CombatCondition.Disengaging]: { label: 'Disengage', color: 'bg-gray-500' },
  [CombatCondition.Hidden]: { label: 'Hidden', color: 'bg-purple-600' },
  [CombatCondition.HasAdvantage]: { label: 'Advantage', color: 'bg-green-600' },
  [CombatCondition.HasDisadvantage]: { label: 'Disadvantage', color: 'bg-red-600' },
};

export function CombatParticipantCard({ participant, isActive, isSelected, onClick }: CombatParticipantCardProps) {
  const isEnemy = participant.participantType === 2;
  const isDead = !participant.isAlive;
  const conditions = participant.activeConditions ?? [];

  return (
    <div
      onClick={onClick}
      className={`relative p-3 rounded-lg border-2 transition-all min-w-[140px]
        ${isDead ? 'opacity-40 border-gray-700 bg-gray-800/50' : ''}
        ${!isDead && isSelected ? 'border-amber-400 bg-gray-700 shadow-lg shadow-amber-400/10' : ''}
        ${!isDead && !isSelected && isActive ? 'border-amber-500/50 bg-gray-700' : ''}
        ${!isDead && !isSelected && !isActive ? 'border-gray-600 bg-gray-800' : ''}
        ${!isDead && onClick ? 'cursor-pointer hover:border-amber-400 hover:bg-gray-700' : ''}
      `}
    >
      {isActive && !isDead && (
        <div className="absolute -top-2 left-1/2 -translate-x-1/2 px-1.5 py-0.5 bg-amber-500 text-black text-[10px] font-bold rounded">
          TURN
        </div>
      )}

      <div className="flex items-center gap-2 mb-2">
        <div className={`w-8 h-8 rounded-full flex items-center justify-center text-sm font-bold border-2
          ${isDead ? 'bg-gray-700 border-gray-600 text-gray-500' : ''}
          ${!isDead && isEnemy ? 'bg-red-900 border-red-500 text-red-300' : ''}
          ${!isDead && !isEnemy ? 'bg-blue-900 border-blue-500 text-blue-300' : ''}
        `}>
          {isDead ? 'X' : isEnemy ? 'E' : 'P'}
        </div>
        <div>
          <p className={`text-sm font-bold ${isDead ? 'text-gray-500 line-through' : 'text-white'}`}>
            {participant.name}
          </p>
          <p className="text-xs text-gray-400">AC {participant.armorClass}</p>
        </div>
      </div>

      <HpBar current={participant.currentHp} max={participant.maxHp} />

      {conditions.length > 0 && !isDead && (
        <div className="flex flex-wrap gap-1 mt-1.5">
          {conditions.map((c) => {
            const info = conditionLabels[c];
            if (!info) return null;
            return (
              <span key={c} className={`${info.color} text-white text-[9px] font-bold px-1.5 py-0.5 rounded`}>
                {info.label}
              </span>
            );
          })}
        </div>
      )}

      {isDead && (
        <p className="text-center text-xs text-red-400 mt-1 font-medium">Defeated</p>
      )}
    </div>
  );
}
