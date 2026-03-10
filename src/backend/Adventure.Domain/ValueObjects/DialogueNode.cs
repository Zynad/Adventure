namespace Adventure.Domain.ValueObjects;

public record DialogueNode(
    string Id,
    string Text,
    string? SpeakerName,
    IReadOnlyList<DialogueOption> Options);

public record DialogueOption(
    string Text,
    string? NextNodeId,
    string? Condition);
