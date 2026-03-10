import { useEffect, useRef } from 'react';

interface CombatLogPanelProps {
  log: string[];
}

function colorizeEntry(entry: string): string {
  if (entry.includes('CRITICAL')) return 'text-orange-400 font-bold';
  if (entry.includes('Victory')) return 'text-green-400 font-bold';
  if (entry.includes('Defeat') || entry.includes('fallen')) return 'text-red-400 font-bold';
  if (entry.includes('restores') || entry.includes('heal')) return 'text-green-300';
  if (entry.includes('hits') || entry.includes('damage')) return 'text-red-300';
  if (entry.includes('misses')) return 'text-yellow-300';
  if (entry.includes('Dodge') || entry.includes('dodges') || entry.includes('bracing')) return 'text-blue-300';
  if (entry.includes('disengage') || entry.includes('retreating')) return 'text-gray-400';
  if (entry.includes('helps') || entry.includes('granting advantage')) return 'text-green-300';
  if (entry.includes('hides') || entry.includes('Stealth') || entry.includes('hide')) return 'text-purple-300';
  if (entry.includes('uses') || entry.includes('drinks')) return 'text-amber-300';
  return 'text-gray-300';
}

export function CombatLogPanel({ log }: CombatLogPanelProps) {
  const endRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    endRef.current?.scrollIntoView({ behavior: 'smooth' });
  }, [log.length]);

  return (
    <div className="bg-gray-900 rounded-lg border border-gray-700 p-3 h-40 overflow-y-auto">
      <p className="text-xs text-gray-500 font-bold mb-1">COMBAT LOG</p>
      {log.map((entry, i) => (
        <p key={i} className={`text-xs leading-relaxed ${colorizeEntry(entry)}`}>
          {entry}
        </p>
      ))}
      <div ref={endRef} />
    </div>
  );
}
