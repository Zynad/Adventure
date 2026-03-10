interface HpBarProps {
  current: number;
  max: number;
  showText?: boolean;
}

export function HpBar({ current, max, showText = true }: HpBarProps) {
  const percentage = max > 0 ? (current / max) * 100 : 0;

  const barColor =
    percentage > 60
      ? 'bg-green-500'
      : percentage > 30
        ? 'bg-yellow-500'
        : 'bg-red-500';

  return (
    <div className="w-full">
      <div className="h-3 bg-gray-700 rounded-full overflow-hidden">
        <div
          className={`h-full ${barColor} transition-all duration-300`}
          style={{ width: `${percentage}%` }}
        />
      </div>
      {showText && (
        <p className="text-xs text-gray-400 text-center mt-0.5">
          {current} / {max}
        </p>
      )}
    </div>
  );
}
