using System.Text.Json;
using Adventure.Domain.Interfaces;
using Adventure.Domain.ValueObjects;
using MediatR;

namespace Adventure.Application.Features.Dialogue.Queries.GetNpcDialogue;

public class GetNpcDialogueQueryHandler : IRequestHandler<GetNpcDialogueQuery, DialogueNodeDto>
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    private readonly IRepository<Domain.Entities.Npc> _npcRepo;

    public GetNpcDialogueQueryHandler(IRepository<Domain.Entities.Npc> npcRepo)
    {
        _npcRepo = npcRepo;
    }

    public async Task<DialogueNodeDto> Handle(GetNpcDialogueQuery request, CancellationToken cancellationToken)
    {
        var npc = await _npcRepo.GetByIdAsync(request.NpcId, cancellationToken)
            ?? throw new KeyNotFoundException($"NPC with id {request.NpcId} not found.");

        if (string.IsNullOrWhiteSpace(npc.DialogueTree))
            return new DialogueNodeDto("end", $"{npc.Name} has nothing to say.", npc.Name, [], true);

        var nodes = JsonSerializer.Deserialize<List<DialogueNode>>(npc.DialogueTree, JsonOptions)
            ?? throw new InvalidOperationException($"Failed to parse dialogue tree for NPC {npc.Name}.");

        var rootNode = nodes.FirstOrDefault(n => n.Id == "root") ?? nodes[0];

        return ToDto(rootNode, npc.Name);
    }

    internal static DialogueNodeDto ToDto(DialogueNode node, string npcName)
    {
        var options = node.Options
            .Select((o, i) => new DialogueOptionDto(i, o.Text))
            .ToList();

        var isEnd = options.Count == 0;

        return new DialogueNodeDto(
            node.Id,
            node.Text,
            node.SpeakerName ?? npcName,
            options,
            isEnd);
    }
}
