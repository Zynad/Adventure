import { useState } from 'react';
import { advanceDialogue } from '../../api/npcApi';
import { useGameStore } from '../../store/gameStore';

export function DialogueModal() {
  const activeDialogue = useGameStore((s) => s.activeDialogue);
  const setActiveDialogue = useGameStore((s) => s.setActiveDialogue);
  const addNarrativeEvent = useGameStore((s) => s.addNarrativeEvent);
  const [loading, setLoading] = useState(false);

  if (!activeDialogue) return null;

  const { npcId, npcName, node } = activeDialogue;

  const handleOption = async (optionIndex: number) => {
    if (loading) return;
    setLoading(true);
    try {
      const { data } = await advanceDialogue(npcId, node.nodeId, optionIndex);
      if (data.isEnd) {
        setActiveDialogue(null);
        addNarrativeEvent(`You spoke with ${npcName}.`);
      } else {
        setActiveDialogue({ npcId, npcName, node: data });
      }
    } catch {
      setActiveDialogue(null);
    } finally {
      setLoading(false);
    }
  };

  const handleClose = () => {
    setActiveDialogue(null);
    addNarrativeEvent(`You spoke with ${npcName}.`);
  };

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/60">
      <div className="bg-gray-800 border border-gray-600 rounded-lg shadow-2xl max-w-md w-full mx-4">
        <div className="border-b border-gray-700 px-4 py-3 flex items-center justify-between">
          <h3 className="text-amber-400 font-bold text-lg">{node.speakerName}</h3>
          <button
            onClick={handleClose}
            className="text-gray-400 hover:text-white text-xl leading-none"
          >
            &times;
          </button>
        </div>

        <div className="px-4 py-4">
          <p className="text-gray-200 leading-relaxed">{node.text}</p>
        </div>

        <div className="px-4 pb-4 space-y-2">
          {node.isEnd ? (
            <button
              onClick={handleClose}
              className="w-full text-left px-3 py-2 bg-gray-700 hover:bg-gray-600 rounded text-sm text-gray-200 transition-colors"
            >
              [End conversation]
            </button>
          ) : (
            node.options.map((option) => (
              <button
                key={option.index}
                onClick={() => handleOption(option.index)}
                disabled={loading}
                className="w-full text-left px-3 py-2 bg-gray-700 hover:bg-amber-900/50 hover:text-amber-200 rounded text-sm text-gray-200 transition-colors disabled:opacity-50"
              >
                {option.text}
              </button>
            ))
          )}
        </div>
      </div>
    </div>
  );
}
