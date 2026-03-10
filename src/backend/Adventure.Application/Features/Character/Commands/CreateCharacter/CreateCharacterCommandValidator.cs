using Adventure.Domain.Rules;
using FluentValidation;

namespace Adventure.Application.Features.Character.Commands.CreateCharacter;

public class CreateCharacterCommandValidator : AbstractValidator<CreateCharacterCommand>
{
    public CreateCharacterCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Character name is required.")
            .MinimumLength(2).WithMessage("Name must be at least 2 characters.")
            .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");

        RuleFor(x => x.Race).IsInEnum().WithMessage("Invalid race.");
        RuleFor(x => x.CharacterClass).IsInEnum().WithMessage("Invalid class.");

        RuleFor(x => x.Strength).InclusiveBetween(8, 15).WithMessage("Ability scores must be between 8 and 15 (Point Buy).");
        RuleFor(x => x.Dexterity).InclusiveBetween(8, 15).WithMessage("Ability scores must be between 8 and 15 (Point Buy).");
        RuleFor(x => x.Constitution).InclusiveBetween(8, 15).WithMessage("Ability scores must be between 8 and 15 (Point Buy).");
        RuleFor(x => x.Intelligence).InclusiveBetween(8, 15).WithMessage("Ability scores must be between 8 and 15 (Point Buy).");
        RuleFor(x => x.Wisdom).InclusiveBetween(8, 15).WithMessage("Ability scores must be between 8 and 15 (Point Buy).");
        RuleFor(x => x.Charisma).InclusiveBetween(8, 15).WithMessage("Ability scores must be between 8 and 15 (Point Buy).");

        RuleFor(x => x)
            .Must(x => PointBuyCostTable.IsValid(x.Strength, x.Dexterity, x.Constitution,
                x.Intelligence, x.Wisdom, x.Charisma))
            .WithMessage($"Total point buy cost exceeds {PointBuyCostTable.TotalPoints} points.");
    }
}
