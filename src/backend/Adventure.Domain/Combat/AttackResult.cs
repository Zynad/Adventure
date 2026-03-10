namespace Adventure.Domain.Combat;

public record AttackResult(int NaturalRoll, int TotalRoll, bool IsHit, bool IsCritical);
