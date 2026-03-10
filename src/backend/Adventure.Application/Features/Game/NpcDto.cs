namespace Adventure.Application.Features.Game;

public record NpcDto(
    Guid Id,
    string Name,
    string Description,
    int PositionX,
    int PositionY,
    bool IsQuestGiver,
    bool IsMerchant);
