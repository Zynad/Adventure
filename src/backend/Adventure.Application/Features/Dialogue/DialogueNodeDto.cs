namespace Adventure.Application.Features.Dialogue;

public record DialogueNodeDto(
    string NodeId,
    string Text,
    string SpeakerName,
    IReadOnlyList<DialogueOptionDto> Options,
    bool IsEnd);

public record DialogueOptionDto(
    int Index,
    string Text);
