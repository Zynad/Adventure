import { HpBar } from '../../components/HpBar';
import { useGameStore } from '../../store/gameStore';
import { RaceLabels, ClassLabels } from '../../types';

export function CharacterSidebar() {
  const character = useGameStore((s) => s.character);

  if (!character) return null;

  const abilityMod = (score: number) => {
    const mod = Math.floor((score - 10) / 2);
    return mod >= 0 ? `+${mod}` : `${mod}`;
  };

  const abilities = [
    { label: 'STR', value: character.strength },
    { label: 'DEX', value: character.dexterity },
    { label: 'CON', value: character.constitution },
    { label: 'INT', value: character.intelligence },
    { label: 'WIS', value: character.wisdom },
    { label: 'CHA', value: character.charisma },
  ];

  return (
    <div className="bg-gray-800 rounded-lg p-4 space-y-4">
      <div>
        <h2 className="text-lg font-bold text-amber-500">{character.name}</h2>
        <p className="text-sm text-gray-400">
          {RaceLabels[character.race]} {ClassLabels[character.characterClass]}
        </p>
        <p className="text-sm text-gray-400">Level {character.level}</p>
      </div>

      <div>
        <div className="flex justify-between text-sm text-gray-300 mb-1">
          <span>HP</span>
          <span>AC {character.armorClass}</span>
        </div>
        <HpBar current={character.currentHitPoints} max={character.maxHitPoints} />
      </div>

      <div>
        <p className="text-xs text-gray-500 mb-1">XP: {character.experiencePoints}</p>
        <p className="text-xs text-gray-500">Gold: {character.gold} gp</p>
      </div>

      <div className="grid grid-cols-3 gap-2">
        {abilities.map((a) => (
          <div key={a.label} className="bg-gray-700 rounded p-2 text-center">
            <p className="text-xs text-gray-400">{a.label}</p>
            <p className="text-sm font-bold">{a.value}</p>
            <p className="text-xs text-gray-500">{abilityMod(a.value)}</p>
          </div>
        ))}
      </div>

      {character.spellSlots && character.spellSlots.length > 0 && (
        <div>
          <p className="text-xs text-gray-400 font-semibold mb-1">Spell Slots</p>
          <div className="space-y-1">
            {character.spellSlots.map((slot) => (
              <div key={slot.level} className="flex items-center gap-2 text-xs">
                <span className="text-gray-500 w-10">Lvl {slot.level}</span>
                <div className="flex gap-0.5">
                  {Array.from({ length: slot.maxSlots }, (_, i) => (
                    <span
                      key={i}
                      className={`w-3 h-3 rounded-full border ${
                        i < slot.currentSlots
                          ? 'bg-indigo-500 border-indigo-400'
                          : 'bg-gray-700 border-gray-600'
                      }`}
                    />
                  ))}
                </div>
              </div>
            ))}
          </div>
        </div>
      )}
    </div>
  );
}
