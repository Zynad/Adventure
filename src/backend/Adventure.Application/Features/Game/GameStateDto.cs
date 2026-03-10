using Adventure.Application.Features.Character;
using Adventure.Application.Features.World;

namespace Adventure.Application.Features.Game;

public record GameStateDto(
    CharacterDto Character,
    ZoneDto CurrentZone,
    IReadOnlyList<TileDto> Tiles,
    IReadOnlyList<NpcDto> NpcsInZone);
