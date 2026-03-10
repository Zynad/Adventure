using MediatR;

namespace Adventure.Application.Features.Dialogue.Commands.AdvanceDialogue;

public record AdvanceDialogueCommand(
    Guid NpcId,
    string CurrentNodeId,
    int ChosenOptionIndex) : IRequest<DialogueNodeDto>;
