using Adventure.Domain.Entities;
using Adventure.Domain.Enums;
using Adventure.Domain.Interfaces;
using MediatR;

namespace Adventure.Application.Features.Combat.Queries.GetCombatConsumables;

public class GetCombatConsumablesQueryHandler : IRequestHandler<GetCombatConsumablesQuery, IReadOnlyList<CombatConsumableDto>>
{
    private readonly IRepository<InventorySlot> _inventoryRepo;
    private readonly IRepository<Item> _itemRepo;

    public GetCombatConsumablesQueryHandler(
        IRepository<InventorySlot> inventoryRepo,
        IRepository<Item> itemRepo)
    {
        _inventoryRepo = inventoryRepo;
        _itemRepo = itemRepo;
    }

    public async Task<IReadOnlyList<CombatConsumableDto>> Handle(
        GetCombatConsumablesQuery request, CancellationToken cancellationToken)
    {
        var slots = await _inventoryRepo.FindAsync(
            s => s.CharacterId == request.CharacterId, cancellationToken);

        var consumables = new List<CombatConsumableDto>();

        foreach (var slot in slots)
        {
            var item = await _itemRepo.GetByIdAsync(slot.ItemId, cancellationToken);
            if (item is Consumable consumable)
            {
                consumables.Add(new CombatConsumableDto(
                    consumable.Id,
                    consumable.Name,
                    consumable.EffectType,
                    consumable.EffectValue,
                    slot.Quantity));
            }
        }

        return consumables;
    }
}
