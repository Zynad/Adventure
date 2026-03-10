import { useGameStore } from '../../store/gameStore';
import { TileType } from '../../types';
import type { TileDto, NpcDto } from '../../types';

const TILE_COLORS: Record<TileType, string> = {
  [TileType.Grass]: 'bg-green-700',
  [TileType.Stone]: 'bg-gray-500',
  [TileType.Water]: 'bg-blue-600',
  [TileType.Wall]: 'bg-gray-800',
  [TileType.Door]: 'bg-amber-800',
  [TileType.Sand]: 'bg-yellow-600',
  [TileType.Dirt]: 'bg-amber-900',
  [TileType.Wood]: 'bg-yellow-900',
  [TileType.Bridge]: 'bg-amber-700',
  [TileType.StairsUp]: 'bg-gray-600',
  [TileType.StairsDown]: 'bg-gray-700',
  [TileType.Encounter]: 'bg-red-900/70',
};

interface TileMapProps {
  width: number;
  height: number;
  onTileClick?: (dx: number, dy: number) => void;
  onNpcClick?: (npc: NpcDto) => void;
}

export function TileMap({ width, height, onTileClick, onNpcClick }: TileMapProps) {
  const tiles = useGameStore((s) => s.tiles);
  const character = useGameStore((s) => s.character);
  const npcsInZone = useGameStore((s) => s.npcsInZone);

  const tileGrid = new Map<string, TileDto>();
  for (const tile of tiles) {
    tileGrid.set(`${tile.x},${tile.y}`, tile);
  }

  const npcPositions = new Map<string, NpcDto>();
  for (const npc of npcsInZone) {
    npcPositions.set(`${npc.positionX},${npc.positionY}`, npc);
  }

  const isAdjacent = (x: number, y: number) => {
    if (!character) return false;
    return Math.abs(x - character.positionX) + Math.abs(y - character.positionY) === 1;
  };

  const handleClick = (x: number, y: number, tile: TileDto | undefined, npc: NpcDto | undefined) => {
    if (!character || !isAdjacent(x, y)) return;

    if (npc && onNpcClick) {
      onNpcClick(npc);
      return;
    }

    if (tile?.isWalkable && onTileClick) {
      const dx = x - character.positionX;
      const dy = y - character.positionY;
      onTileClick(dx, dy);
    }
  };

  const cells = [];
  for (let y = 0; y < height; y++) {
    for (let x = 0; x < width; x++) {
      const key = `${x},${y}`;
      const tile = tileGrid.get(key);
      const npc = npcPositions.get(key);
      const isPlayer = character?.positionX === x && character?.positionY === y;
      const adjacent = isAdjacent(x, y);
      const canClick = adjacent && (tile?.isWalkable || npc);

      const bgColor = tile ? TILE_COLORS[tile.tileType as TileType] ?? 'bg-gray-900' : 'bg-gray-900';

      let borderClass = 'border border-gray-900/30';
      if (adjacent && npc) {
        borderClass = 'border-2 border-amber-400/60';
      } else if (adjacent && tile?.isWalkable) {
        borderClass = 'border border-cyan-500/40';
      }

      cells.push(
        <div
          key={key}
          className={`relative aspect-square ${bgColor} ${borderClass} ${canClick ? 'cursor-pointer hover:brightness-125' : ''}`}
          title={`(${x}, ${y})${tile ? ` - ${TileType[tile.tileType]}` : ''}${npc ? ` - ${npc.name}` : ''}`}
          onClick={() => handleClick(x, y, tile, npc)}
        >
          {isPlayer && (
            <div className="absolute inset-0 flex items-center justify-center">
              <div className="w-3/5 h-3/5 bg-amber-400 rounded-full border-2 border-amber-300 shadow-lg shadow-amber-400/50" />
            </div>
          )}
          {npc && !isPlayer && (
            <div className="absolute inset-0 flex items-center justify-center">
              <div className={`w-3/5 h-3/5 rounded-full border-2 ${
                npc.isQuestGiver ? 'bg-yellow-500 border-yellow-400' : 'bg-blue-400 border-blue-300'
              } ${adjacent ? 'animate-pulse' : ''}`} />
            </div>
          )}
          {tile?.isInteractable && !npc && !isPlayer && (
            <div className="absolute top-0.5 right-0.5 w-1.5 h-1.5 bg-amber-400 rounded-full opacity-70" />
          )}
        </div>
      );
    }
  }

  return (
    <div className="bg-gray-900 rounded-lg p-2 overflow-auto max-h-[calc(100vh-2rem)]">
      <div
        className="grid gap-0"
        style={{
          gridTemplateColumns: `repeat(${width}, minmax(2.5rem, 1fr))`,
        }}
      >
        {cells}
      </div>
      <div className="flex gap-4 mt-2 text-xs text-gray-500 justify-center flex-wrap">
        <span className="flex items-center gap-1">
          <span className="w-3 h-3 bg-amber-400 rounded-full inline-block" /> You
        </span>
        <span className="flex items-center gap-1">
          <span className="w-3 h-3 bg-yellow-500 rounded-full inline-block" /> Quest NPC
        </span>
        <span className="flex items-center gap-1">
          <span className="w-3 h-3 bg-blue-400 rounded-full inline-block" /> Merchant
        </span>
      </div>
    </div>
  );
}
