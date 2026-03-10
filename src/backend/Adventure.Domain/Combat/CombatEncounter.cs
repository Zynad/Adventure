using Adventure.Domain.Enums;

namespace Adventure.Domain.Combat;

public class CombatEncounter
{
    private readonly List<CombatParticipant> _participants = [];
    private readonly List<string> _combatLog = [];
    private List<CombatParticipant>? _turnOrderCache;

    public Guid Id { get; private set; }
    public Guid CharacterId { get; private set; }
    public int CurrentRound { get; private set; } = 1;
    public int CurrentTurnIndex { get; private set; }
    public bool IsComplete { get; private set; }
    public bool HasTakenAction { get; private set; }
    public CombatOutcome? Outcome { get; private set; }

    public IReadOnlyList<CombatParticipant> Participants => _participants.AsReadOnly();
    public IReadOnlyList<string> CombatLog => _combatLog.AsReadOnly();

    public IReadOnlyList<CombatParticipant> TurnOrder
    {
        get
        {
            _turnOrderCache ??= [.. _participants
                .OrderByDescending(p => p.InitiativeRoll)
                .ThenByDescending(p => p.Dexterity)
                .ThenBy(p => p.Name)];
            return _turnOrderCache.AsReadOnly();
        }
    }

    public CombatParticipant ActiveParticipant => TurnOrder[CurrentTurnIndex];

    private CombatEncounter() { }

    public static CombatEncounter Create(Guid characterId)
    {
        return new CombatEncounter
        {
            Id = Guid.NewGuid(),
            CharacterId = characterId
        };
    }

    public void AddParticipant(CombatParticipant participant)
    {
        _participants.Add(participant);
        _turnOrderCache = null;
    }

    public void SetActionTaken()
    {
        HasTakenAction = true;
    }

    public void AdvanceTurn()
    {
        if (IsComplete) return;

        var turnOrder = TurnOrder;
        var startIndex = CurrentTurnIndex;

        do
        {
            CurrentTurnIndex = (CurrentTurnIndex + 1) % turnOrder.Count;
            if (CurrentTurnIndex == 0)
                CurrentRound++;
        }
        while (!turnOrder[CurrentTurnIndex].IsAlive && CurrentTurnIndex != startIndex);

        HasTakenAction = false;
        turnOrder[CurrentTurnIndex].ClearConditions();
    }

    public void AddLogEntry(string message)
    {
        _combatLog.Add(message);
    }

    public void EndCombat(CombatOutcome outcome)
    {
        IsComplete = true;
        Outcome = outcome;
    }

    public CombatParticipant GetParticipant(Guid id)
    {
        return _participants.Find(p => p.Id == id)
            ?? throw new KeyNotFoundException($"Participant with id {id} not found in combat.");
    }

    public bool AreAllEnemiesDead()
    {
        return _participants
            .Where(p => p.ParticipantType == ParticipantType.Enemy)
            .All(p => !p.IsAlive);
    }

    public bool IsPlayerDead()
    {
        return _participants
            .Where(p => p.ParticipantType == ParticipantType.Player)
            .All(p => !p.IsAlive);
    }
}
