import { CombatActionType } from '../../types';

interface CombatActionMenuProps {
  isPlayerTurn: boolean;
  hasTakenAction: boolean;
  isComplete: boolean;
  isProcessing: boolean;
  selectedAction: number | null;
  isSpellcaster: boolean;
  onSelectAction: (action: CombatActionType) => void;
}

const actions = [
  { type: CombatActionType.Attack, label: 'Attack', color: 'bg-red-700 hover:bg-red-600', needsTarget: true, targetType: 'enemy' as const },
  { type: CombatActionType.CastSpell, label: 'Cast Spell', color: 'bg-indigo-700 hover:bg-indigo-600', needsTarget: false, spellcasterOnly: true },
  { type: CombatActionType.Dodge, label: 'Dodge', color: 'bg-blue-700 hover:bg-blue-600', needsTarget: false },
  { type: CombatActionType.Disengage, label: 'Disengage', color: 'bg-gray-600 hover:bg-gray-500', needsTarget: false },
  { type: CombatActionType.Help, label: 'Help', color: 'bg-green-700 hover:bg-green-600', needsTarget: true, targetType: 'ally' as const },
  { type: CombatActionType.Hide, label: 'Hide', color: 'bg-purple-700 hover:bg-purple-600', needsTarget: false },
  { type: CombatActionType.UseItem, label: 'Use Item', color: 'bg-amber-700 hover:bg-amber-600', needsTarget: false },
] as const;

export function CombatActionMenu({
  isPlayerTurn,
  hasTakenAction,
  isComplete,
  isProcessing,
  selectedAction,
  isSpellcaster,
  onSelectAction,
}: CombatActionMenuProps) {
  const disabled = !isPlayerTurn || hasTakenAction || isComplete || isProcessing;

  return (
    <div className="flex flex-wrap gap-2">
      {actions.map((action) => {
        if ('spellcasterOnly' in action && action.spellcasterOnly && !isSpellcaster) return null;

        const isSelected = selectedAction === action.type;
        return (
          <button
            key={action.type}
            onClick={() => onSelectAction(action.type)}
            disabled={disabled}
            className={`px-4 py-2 text-sm font-bold rounded-lg transition-colors text-white
              ${isSelected ? 'ring-2 ring-white ring-offset-2 ring-offset-gray-800' : ''}
              ${disabled ? 'opacity-40 cursor-not-allowed bg-gray-700' : action.color}`}
          >
            {action.label}
          </button>
        );
      })}
    </div>
  );
}

export function getActionTargetType(action: CombatActionType): 'enemy' | 'ally' | null {
  switch (action) {
    case CombatActionType.Attack:
      return 'enemy';
    case CombatActionType.Help:
      return 'ally';
    default:
      return null;
  }
}
