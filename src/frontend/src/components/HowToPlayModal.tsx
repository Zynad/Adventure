interface HowToPlayModalProps {
  onClose: () => void;
}

export function HowToPlayModal({ onClose }: HowToPlayModalProps) {
  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/60" onClick={onClose}>
      <div
        className="bg-gray-800 border border-gray-600 rounded-lg shadow-2xl max-w-lg w-full mx-4"
        onClick={(e) => e.stopPropagation()}
      >
        <div className="border-b border-gray-700 px-4 py-3 flex items-center justify-between">
          <h3 className="text-amber-400 font-bold text-lg">How to Play</h3>
          <button onClick={onClose} className="text-gray-400 hover:text-white text-xl leading-none">
            &times;
          </button>
        </div>

        <div className="px-4 py-4 space-y-4 text-sm text-gray-300">
          <div>
            <h4 className="font-semibold text-gray-200 mb-1">Movement</h4>
            <p>Use <kbd className="px-1.5 py-0.5 bg-gray-700 rounded text-xs font-mono">W</kbd> <kbd className="px-1.5 py-0.5 bg-gray-700 rounded text-xs font-mono">A</kbd> <kbd className="px-1.5 py-0.5 bg-gray-700 rounded text-xs font-mono">S</kbd> <kbd className="px-1.5 py-0.5 bg-gray-700 rounded text-xs font-mono">D</kbd> or arrow keys to move in four directions. You can also click adjacent tiles on the map.</p>
          </div>

          <div>
            <h4 className="font-semibold text-gray-200 mb-1">NPC Interaction</h4>
            <p>Click on an adjacent NPC (pulsing circle) on the map to start a conversation. Choose dialogue options to learn about the world and its inhabitants.</p>
          </div>

          <div>
            <h4 className="font-semibold text-gray-200 mb-1">Map Legend</h4>
            <ul className="space-y-1 ml-2">
              <li className="flex items-center gap-2"><span className="w-3 h-3 bg-amber-400 rounded-full inline-block" /> Your character</li>
              <li className="flex items-center gap-2"><span className="w-3 h-3 bg-yellow-500 rounded-full inline-block" /> Quest giver NPC</li>
              <li className="flex items-center gap-2"><span className="w-3 h-3 bg-blue-400 rounded-full inline-block" /> Merchant NPC</li>
              <li className="flex items-center gap-2"><span className="w-1.5 h-1.5 bg-amber-400 rounded-full inline-block" /> Interactable tile</li>
            </ul>
          </div>

          <div>
            <h4 className="font-semibold text-gray-200 mb-1">Zone Travel</h4>
            <p>Walk to the edge of a zone to travel to connected areas. The narrative panel will show where you are.</p>
          </div>
        </div>
      </div>
    </div>
  );
}
