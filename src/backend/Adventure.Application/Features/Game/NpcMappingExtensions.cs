using Adventure.Domain.Entities;

namespace Adventure.Application.Features.Game;

public static class NpcMappingExtensions
{
    public static NpcDto ToDto(this Npc npc)
    {
        return new NpcDto(
            npc.Id,
            npc.Name,
            npc.Description,
            npc.PositionX,
            npc.PositionY,
            npc.IsQuestGiver,
            npc.IsMerchant);
    }
}
