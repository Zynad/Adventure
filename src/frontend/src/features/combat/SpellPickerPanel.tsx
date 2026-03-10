import { useEffect, useState } from 'react';
import { getCombatSpells } from '../../api/combatApi';
import type { CombatSpellInfoDto, CombatSpellDto } from '../../types';

interface SpellPickerPanelProps {
  characterId: string;
  onCastSpell: (spellId: string, targetType: string) => void;
  onClose: () => void;
}

export function SpellPickerPanel({ characterId, onCastSpell, onClose }: SpellPickerPanelProps) {
  const [spellInfo, setSpellInfo] = useState<CombatSpellInfoDto | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    let cancelled = false;
    setLoading(true);
    getCombatSpells(characterId)
      .then(({ data }) => {
        if (!cancelled) setSpellInfo(data);
      })
      .catch(() => {
        if (!cancelled) setSpellInfo(null);
      })
      .finally(() => {
        if (!cancelled) setLoading(false);
      });
    return () => { cancelled = true; };
  }, [characterId]);

  const cantrips = spellInfo?.spells.filter((s) => s.isCantrip) ?? [];
  const leveledSpells = spellInfo?.spells.filter((s) => !s.isCantrip) ?? [];

  return (
    <div className="absolute bottom-full left-0 mb-2 w-80 bg-gray-900 border border-indigo-600/50 rounded-lg shadow-xl p-3 z-10">
      <div className="flex items-center justify-between mb-2">
        <h3 className="text-sm font-bold text-indigo-400">Cast Spell</h3>
        <button
          onClick={onClose}
          className="text-gray-400 hover:text-white text-xs"
        >
          Close
        </button>
      </div>

      {/* Spell Slots */}
      {spellInfo && spellInfo.spellSlots.length > 0 && (
        <div className="flex gap-3 mb-2 text-xs text-gray-400">
          {spellInfo.spellSlots.map((slot) => (
            <span key={slot.level}>
              Lvl {slot.level}: {slot.currentSlots}/{slot.maxSlots}
            </span>
          ))}
        </div>
      )}

      {loading && <p className="text-xs text-gray-500">Loading...</p>}

      {!loading && (!spellInfo || spellInfo.spells.length === 0) && (
        <p className="text-xs text-gray-500">No spells known.</p>
      )}

      {!loading && spellInfo && spellInfo.spells.length > 0 && (
        <div className="flex flex-col gap-1 max-h-48 overflow-y-auto">
          {cantrips.length > 0 && (
            <>
              <p className="text-xs text-gray-500 font-semibold mt-1">Cantrips</p>
              {cantrips.map((spell) => (
                <SpellButton key={spell.spellId} spell={spell} onCast={onCastSpell} />
              ))}
            </>
          )}
          {leveledSpells.length > 0 && (
            <>
              <p className="text-xs text-gray-500 font-semibold mt-1">Spells</p>
              {leveledSpells.map((spell) => (
                <SpellButton key={spell.spellId} spell={spell} onCast={onCastSpell} />
              ))}
            </>
          )}
        </div>
      )}
    </div>
  );
}

function SpellButton({ spell, onCast }: { spell: CombatSpellDto; onCast: (id: string, target: string) => void }) {
  const effectText = spell.damageDice
    ? spell.damageDice
    : spell.healingDice
      ? `Heal ${spell.healingDice}`
      : 'Buff';

  return (
    <button
      onClick={() => onCast(spell.spellId, spell.targetType)}
      disabled={!spell.canCast}
      className={`flex items-center justify-between px-2 py-1.5 text-left rounded transition-colors
        ${spell.canCast
          ? 'hover:bg-indigo-900/50'
          : 'opacity-40 cursor-not-allowed'
        }`}
    >
      <div className="flex items-center gap-2">
        <span className="text-sm text-white">{spell.name}</span>
        <span className="text-xs text-gray-400">{effectText}</span>
      </div>
      <span className="text-xs text-indigo-400">
        {spell.isCantrip ? '\u221E' : `Lvl ${spell.spellLevel}`}
      </span>
    </button>
  );
}
