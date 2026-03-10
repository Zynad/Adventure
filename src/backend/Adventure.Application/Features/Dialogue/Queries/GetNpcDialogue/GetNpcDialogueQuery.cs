using MediatR;

namespace Adventure.Application.Features.Dialogue.Queries.GetNpcDialogue;

public record GetNpcDialogueQuery(Guid NpcId) : IRequest<DialogueNodeDto>;
