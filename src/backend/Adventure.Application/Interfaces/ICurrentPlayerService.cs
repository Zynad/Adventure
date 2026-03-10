namespace Adventure.Application.Interfaces;

public interface ICurrentPlayerService
{
    Guid? GetCurrentCharacterId();
    void SetCurrentCharacterId(Guid characterId);
}
