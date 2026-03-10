using System.Collections.Concurrent;
using Adventure.Domain.Combat;

namespace Adventure.Application.Features.Combat;

public class CombatEncounterStore : ICombatEncounterStore
{
    private readonly ConcurrentDictionary<Guid, CombatEncounter> _encounters = new();

    public CombatEncounter? GetByCharacterId(Guid characterId)
    {
        _encounters.TryGetValue(characterId, out var encounter);
        return encounter;
    }

    public void Add(Guid characterId, CombatEncounter encounter)
    {
        if (!_encounters.TryAdd(characterId, encounter))
            throw new InvalidOperationException($"Character {characterId} already has an active combat encounter.");
    }

    public void Remove(Guid characterId)
    {
        _encounters.TryRemove(characterId, out _);
    }

    public bool HasActiveEncounter(Guid characterId)
    {
        return _encounters.ContainsKey(characterId);
    }
}
