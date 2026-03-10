namespace Adventure.Domain.ValueObjects;

public record SpellSlot(int Level, int MaxSlots, int CurrentSlots)
{
    public bool HasAvailableSlot => CurrentSlots > 0;

    public SpellSlot UseSlot() => this with { CurrentSlots = Math.Max(0, CurrentSlots - 1) };

    public SpellSlot RestoreAll() => this with { CurrentSlots = MaxSlots };
}
