using Adventure.Domain.Enums;

namespace Adventure.Domain.Entities;

public class SaveGame
{
    public Guid Id { get; private set; }
    public Guid CharacterId { get; private set; }
    public int SlotNumber { get; private set; }
    public string SaveName { get; private set; } = string.Empty;
    public SaveType SaveType { get; private set; }
    public string GameStateJson { get; private set; } = string.Empty;
    public TimeSpan PlayTime { get; private set; }
    public DateTime SavedAt { get; private set; }

    protected SaveGame() { }
}
