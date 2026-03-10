using Adventure.Domain.Enums;
using MediatR;

namespace Adventure.Application.Features.Character.Commands.CreateCharacter;

public record CreateCharacterCommand(
    string Name,
    Race Race,
    CharacterClass CharacterClass,
    int Strength,
    int Dexterity,
    int Constitution,
    int Intelligence,
    int Wisdom,
    int Charisma) : IRequest<CharacterDto>;
