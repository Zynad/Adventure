import { useEffect, useState } from 'react';
import { getCombatConsumables } from '../../api/combatApi';
import type { CombatConsumableDto } from '../../types';

interface ItemPickerPanelProps {
  characterId: string;
  onUseItem: (itemId: string) => void;
  onClose: () => void;
}

const effectLabels: Record<string, string> = {
  HealthRestore: 'Restores HP',
};

export function ItemPickerPanel({ characterId, onUseItem, onClose }: ItemPickerPanelProps) {
  const [consumables, setConsumables] = useState<CombatConsumableDto[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    let cancelled = false;
    setLoading(true);
    getCombatConsumables(characterId)
      .then(({ data }) => {
        if (!cancelled) setConsumables(data);
      })
      .catch(() => {
        if (!cancelled) setConsumables([]);
      })
      .finally(() => {
        if (!cancelled) setLoading(false);
      });
    return () => { cancelled = true; };
  }, [characterId]);

  return (
    <div className="absolute bottom-full left-0 mb-2 w-72 bg-gray-900 border border-gray-600 rounded-lg shadow-xl p-3 z-10">
      <div className="flex items-center justify-between mb-2">
        <h3 className="text-sm font-bold text-amber-400">Use Item</h3>
        <button
          onClick={onClose}
          className="text-gray-400 hover:text-white text-xs"
        >
          Close
        </button>
      </div>

      {loading && <p className="text-xs text-gray-500">Loading...</p>}

      {!loading && consumables.length === 0 && (
        <p className="text-xs text-gray-500">No consumable items available.</p>
      )}

      {!loading && consumables.length > 0 && (
        <div className="flex flex-col gap-1 max-h-40 overflow-y-auto">
          {consumables.map((item) => (
            <button
              key={item.itemId}
              onClick={() => onUseItem(item.itemId)}
              className="flex items-center justify-between px-2 py-1.5 text-left rounded hover:bg-gray-700 transition-colors"
            >
              <div>
                <span className="text-sm text-white">{item.name}</span>
                <span className="text-xs text-gray-400 ml-2">
                  {effectLabels[item.effectType] ?? item.effectType} ({item.effectValue})
                </span>
              </div>
              <span className="text-xs text-gray-500">x{item.quantity}</span>
            </button>
          ))}
        </div>
      )}
    </div>
  );
}
