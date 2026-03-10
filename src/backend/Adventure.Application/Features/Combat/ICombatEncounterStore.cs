using Adventure.Domain.Combat;

namespace Adventure.Application.Features.Combat;

public interface ICombatEncounterStore
{
    CombatEncounter? GetByCharacterId(Guid characterId);
    void Add(Guid characterId, CombatEncounter encounter);
    void Remove(Guid characterId);
    bool HasActiveEncounter(Guid characterId);
}
