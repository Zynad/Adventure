using System.Text.Json;
using Adventure.Application.Features.Dialogue.Queries.GetNpcDialogue;
using Adventure.Domain.Interfaces;
using Adventure.Domain.ValueObjects;
using MediatR;

namespace Adventure.Application.Features.Dialogue.Commands.AdvanceDialogue;

public class AdvanceDialogueCommandHandler : IRequestHandler<AdvanceDialogueCommand, DialogueNodeDto>
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    private readonly IRepository<Domain.Entities.Npc> _npcRepo;

    public AdvanceDialogueCommandHandler(IRepository<Domain.Entities.Npc> npcRepo)
    {
        _npcRepo = npcRepo;
    }

    public async Task<DialogueNodeDto> Handle(AdvanceDialogueCommand request, CancellationToken cancellationToken)
    {
        var npc = await _npcRepo.GetByIdAsync(request.NpcId, cancellationToken)
            ?? throw new KeyNotFoundException($"NPC with id {request.NpcId} not found.");

        if (string.IsNullOrWhiteSpace(npc.DialogueTree))
            return new DialogueNodeDto("end", $"{npc.Name} has nothing to say.", npc.Name, [], true);

        var nodes = JsonSerializer.Deserialize<List<DialogueNode>>(npc.DialogueTree, JsonOptions)
            ?? throw new InvalidOperationException($"Failed to parse dialogue tree for NPC {npc.Name}.");

        var currentNode = nodes.FirstOrDefault(n => n.Id == request.CurrentNodeId)
            ?? throw new InvalidOperationException($"Dialogue node '{request.CurrentNodeId}' not found.");

        if (request.ChosenOptionIndex < 0 || request.ChosenOptionIndex >= currentNode.Options.Count)
            throw new InvalidOperationException($"Invalid option index {request.ChosenOptionIndex}.");

        var chosenOption = currentNode.Options[request.ChosenOptionIndex];

        if (chosenOption.NextNodeId is null)
            return new DialogueNodeDto("end", "The conversation has ended.", npc.Name, [], true);

        var nextNode = nodes.FirstOrDefault(n => n.Id == chosenOption.NextNodeId)
            ?? throw new InvalidOperationException($"Dialogue node '{chosenOption.NextNodeId}' not found.");

        return GetNpcDialogueQueryHandler.ToDto(nextNode, npc.Name);
    }
}
