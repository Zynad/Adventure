using FluentValidation;

namespace Adventure.Application.Features.Movement.Commands.MoveCharacter;

public class MoveCharacterCommandValidator : AbstractValidator<MoveCharacterCommand>
{
    public MoveCharacterCommandValidator()
    {
        RuleFor(x => x.CharacterId).NotEmpty();
        RuleFor(x => x.Dx).InclusiveBetween(-1, 1);
        RuleFor(x => x.Dy).InclusiveBetween(-1, 1);
        RuleFor(x => x)
            .Must(x => Math.Abs(x.Dx) + Math.Abs(x.Dy) == 1)
            .WithMessage("Must move exactly one tile in a cardinal direction.");
    }
}
