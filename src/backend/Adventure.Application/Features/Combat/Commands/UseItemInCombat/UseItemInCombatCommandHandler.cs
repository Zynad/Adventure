using Adventure.Domain.Entities;
using Adventure.Domain.Enums;
using Adventure.Domain.Interfaces;
using MediatR;

namespace Adventure.Application.Features.Combat.Commands.UseItemInCombat;

public class UseItemInCombatCommandHandler : IRequestHandler<UseItemInCombatCommand, CombatActionResultDto>
{
    private readonly ICombatEncounterStore _encounterStore;
    private readonly IRepository<InventorySlot> _inventoryRepo;
    private readonly IRepository<Item> _itemRepo;
    private readonly IUnitOfWork _unitOfWork;

    public UseItemInCombatCommandHandler(
        ICombatEncounterStore encounterStore,
        IRepository<InventorySlot> inventoryRepo,
        IRepository<Item> itemRepo,
        IUnitOfWork unitOfWork)
    {
        _encounterStore = encounterStore;
        _inventoryRepo = inventoryRepo;
        _itemRepo = itemRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<CombatActionResultDto> Handle(UseItemInCombatCommand request, CancellationToken cancellationToken)
    {
        var encounter = _encounterStore.GetByCharacterId(request.CharacterId)
            ?? throw new KeyNotFoundException("No active combat encounter found.");

        if (encounter.IsComplete)
            throw new InvalidOperationException("Combat has already ended.");

        var active = encounter.ActiveParticipant;
        if (active.ParticipantType != ParticipantType.Player || active.Id != request.CharacterId)
            throw new InvalidOperationException("It is not the player's turn.");

        if (encounter.HasTakenAction)
            throw new InvalidOperationException("You have already taken an action this turn.");

        // Find the inventory slot
        var slots = await _inventoryRepo.FindAsync(
            s => s.CharacterId == request.CharacterId && s.ItemId == request.ItemId, cancellationToken);
        var slot = slots.FirstOrDefault()
            ?? throw new KeyNotFoundException("Item not found in inventory.");

        // Load the item
        var item = await _itemRepo.GetByIdAsync(request.ItemId, cancellationToken)
            ?? throw new KeyNotFoundException("Item not found.");

        if (item.ItemType != ItemType.Consumable)
            throw new InvalidOperationException("Only consumable items can be used in combat.");

        var consumable = item as Consumable
            ?? throw new InvalidOperationException("Item is not a consumable.");

        // Apply effect
        var healingDone = 0;
        string description;

        switch (consumable.EffectType)
        {
            case "HealthRestore":
                var previousHp = active.CurrentHp;
                active.Heal(consumable.EffectValue);
                healingDone = active.CurrentHp - previousHp;
                description = $"{active.Name} uses {consumable.Name} and restores {healingDone} HP! ({active.CurrentHp}/{active.MaxHp})";
                break;
            default:
                description = $"{active.Name} uses {consumable.Name}.";
                break;
        }

        encounter.AddLogEntry(description);

        // Consume the item
        slot.UseOne();
        if (slot.IsEmpty)
            _inventoryRepo.Delete(slot);

        encounter.SetActionTaken();
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CombatActionResultDto(
            (int)CombatActionType.UseItem,
            active.Name, null,
            null, null, null,
            true, false, 0, healingDone, description,
            encounter.ToStateDto());
    }
}
