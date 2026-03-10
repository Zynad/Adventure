import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { createCharacter } from '../../api/characterApi';
import { useGameStore } from '../../store/gameStore';
import { Race, CharacterClass, RaceLabels, ClassLabels } from '../../types';
import type { CreateCharacterRequest } from '../../types';

const ABILITY_NAMES = ['Strength', 'Dexterity', 'Constitution', 'Intelligence', 'Wisdom', 'Charisma'] as const;
type AbilityKey = 'strength' | 'dexterity' | 'constitution' | 'intelligence' | 'wisdom' | 'charisma';

const POINT_BUY_COSTS: Record<number, number> = {
  8: 0, 9: 1, 10: 2, 11: 3, 12: 4, 13: 5, 14: 7, 15: 9,
};

const TOTAL_POINTS = 27;

export function CharacterCreationPage() {
  const navigate = useNavigate();
  const setCharacter = useGameStore((s) => s.setCharacter);

  const [name, setName] = useState('');
  const [race, setRace] = useState<Race>(Race.Human);
  const [charClass, setCharClass] = useState<CharacterClass>(CharacterClass.Fighter);
  const [abilities, setAbilities] = useState({
    strength: 10,
    dexterity: 10,
    constitution: 10,
    intelligence: 10,
    wisdom: 10,
    charisma: 10,
  });
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);

  const totalCost = Object.values(abilities).reduce((sum, v) => sum + (POINT_BUY_COSTS[v] ?? 0), 0);
  const remaining = TOTAL_POINTS - totalCost;

  const canIncrease = (key: AbilityKey) => {
    const current = abilities[key];
    if (current >= 15) return false;
    const nextCost = POINT_BUY_COSTS[current + 1] ?? 0;
    const currentCost = POINT_BUY_COSTS[current] ?? 0;
    return remaining >= (nextCost - currentCost);
  };

  const updateAbility = (key: AbilityKey, delta: number) => {
    setAbilities((prev) => {
      const newValue = prev[key] + delta;
      if (newValue < 8 || newValue > 15) return prev;
      const newAbilities = { ...prev, [key]: newValue };
      const newTotalCost = Object.values(newAbilities).reduce((sum, v) => sum + (POINT_BUY_COSTS[v] ?? 0), 0);
      if (newTotalCost > TOTAL_POINTS) return prev;
      return newAbilities;
    });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setLoading(true);

    const request: CreateCharacterRequest = {
      name,
      race,
      characterClass: charClass,
      ...abilities,
    };

    try {
      const { data } = await createCharacter(request);
      setCharacter(data);
      navigate('/game');
    } catch (err: unknown) {
      if (err && typeof err === 'object' && 'response' in err) {
        const axiosErr = err as { response?: { data?: { message?: string; errors?: Record<string, string[]> } } };
        const errorData = axiosErr.response?.data;
        if (errorData?.errors) {
          const messages = Object.values(errorData.errors).flat().join(', ');
          setError(messages);
        } else {
          setError(errorData?.message ?? 'Failed to create character.');
        }
      } else {
        setError('Failed to create character.');
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen bg-gray-900 text-white flex items-center justify-center p-4">
      <form onSubmit={handleSubmit} className="w-full max-w-lg space-y-6">
        <h1 className="text-4xl font-bold text-center text-amber-500">Create Character</h1>

        {error && (
          <div className="bg-red-900/50 border border-red-500 text-red-300 px-4 py-2 rounded">
            {error}
          </div>
        )}

        <div>
          <label className="block text-sm text-gray-400 mb-1">Name</label>
          <input
            type="text"
            value={name}
            onChange={(e) => setName(e.target.value)}
            className="w-full bg-gray-800 border border-gray-600 rounded px-3 py-2 text-white focus:outline-none focus:border-amber-500"
            placeholder="Enter character name..."
            required
            minLength={2}
            maxLength={50}
          />
        </div>

        <div className="grid grid-cols-2 gap-4">
          <div>
            <label className="block text-sm text-gray-400 mb-1">Race</label>
            <select
              value={race}
              onChange={(e) => setRace(Number(e.target.value))}
              className="w-full bg-gray-800 border border-gray-600 rounded px-3 py-2 text-white focus:outline-none focus:border-amber-500"
            >
              {Object.entries(RaceLabels).map(([value, label]) => (
                <option key={value} value={value}>{label}</option>
              ))}
            </select>
          </div>
          <div>
            <label className="block text-sm text-gray-400 mb-1">Class</label>
            <select
              value={charClass}
              onChange={(e) => setCharClass(Number(e.target.value))}
              className="w-full bg-gray-800 border border-gray-600 rounded px-3 py-2 text-white focus:outline-none focus:border-amber-500"
            >
              {Object.entries(ClassLabels).map(([value, label]) => (
                <option key={value} value={value}>{label}</option>
              ))}
            </select>
          </div>
        </div>

        <div>
          <div className="flex items-center justify-between mb-3">
            <h2 className="text-lg font-semibold text-gray-300">Ability Scores</h2>
            <span className={`text-sm font-bold px-2 py-0.5 rounded ${
              remaining > 5 ? 'text-green-400 bg-green-900/30' :
              remaining > 0 ? 'text-amber-400 bg-amber-900/30' :
              'text-gray-400 bg-gray-700/30'
            }`}>
              {remaining} / {TOTAL_POINTS} points
            </span>
          </div>
          <div className="grid grid-cols-2 gap-3">
            {ABILITY_NAMES.map((abilityName) => {
              const key = abilityName.toLowerCase() as AbilityKey;
              const value = abilities[key];
              const modifier = Math.floor((value - 10) / 2);
              const cost = POINT_BUY_COSTS[value] ?? 0;
              return (
                <div key={key} className="flex items-center justify-between bg-gray-800 rounded px-3 py-2">
                  <div>
                    <span className="text-sm text-gray-300">{abilityName}</span>
                    <span className="text-xs text-gray-500 ml-1">({cost}pt)</span>
                  </div>
                  <div className="flex items-center gap-2">
                    <button
                      type="button"
                      onClick={() => updateAbility(key, -1)}
                      disabled={value <= 8}
                      className="w-7 h-7 bg-gray-700 hover:bg-gray-600 disabled:opacity-30 disabled:cursor-not-allowed rounded text-sm font-bold transition-colors"
                    >
                      -
                    </button>
                    <span className="w-6 text-center font-mono font-bold">{value}</span>
                    <button
                      type="button"
                      onClick={() => updateAbility(key, 1)}
                      disabled={!canIncrease(key)}
                      className="w-7 h-7 bg-gray-700 hover:bg-gray-600 disabled:opacity-30 disabled:cursor-not-allowed rounded text-sm font-bold transition-colors"
                    >
                      +
                    </button>
                    <span className="text-xs text-gray-500 w-8 text-right">
                      ({modifier >= 0 ? '+' : ''}{modifier})
                    </span>
                  </div>
                </div>
              );
            })}
          </div>
        </div>

        <div className="flex gap-4">
          <button
            type="button"
            onClick={() => navigate('/')}
            className="flex-1 px-4 py-3 bg-gray-700 hover:bg-gray-600 rounded-lg font-semibold transition-colors"
          >
            Back
          </button>
          <button
            type="submit"
            disabled={loading}
            className="flex-1 px-4 py-3 bg-amber-700 hover:bg-amber-600 disabled:opacity-50 rounded-lg font-semibold transition-colors"
          >
            {loading ? 'Creating...' : 'Create Character'}
          </button>
        </div>
      </form>
    </div>
  );
}
