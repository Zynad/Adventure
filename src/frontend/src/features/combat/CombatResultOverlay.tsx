interface CombatResultOverlayProps {
  outcome: number; // 0=Victory, 1=Defeat
  onContinue: () => void;
}

export function CombatResultOverlay({ outcome, onContinue }: CombatResultOverlayProps) {
  const isVictory = outcome === 0;

  return (
    <div className="absolute inset-0 z-10 flex items-center justify-center bg-black/70 rounded-lg">
      <div className="text-center">
        <h2 className={`text-5xl font-bold mb-4 ${isVictory ? 'text-amber-400' : 'text-red-500'}`}>
          {isVictory ? 'VICTORY!' : 'DEFEAT'}
        </h2>
        <p className="text-gray-300 mb-6">
          {isVictory
            ? 'All enemies have been defeated.'
            : 'You have fallen in battle...'}
        </p>
        <button
          onClick={onContinue}
          className={`px-6 py-3 rounded-lg font-bold text-lg transition-colors
            ${isVictory
              ? 'bg-amber-600 hover:bg-amber-500 text-white'
              : 'bg-gray-700 hover:bg-gray-600 text-gray-200'
            }`}
        >
          {isVictory ? 'Continue' : 'Return to Menu'}
        </button>
      </div>
    </div>
  );
}
