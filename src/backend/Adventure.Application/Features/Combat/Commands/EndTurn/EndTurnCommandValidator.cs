using FluentValidation;

namespace Adventure.Application.Features.Combat.Commands.EndTurn;

public class EndTurnCommandValidator : AbstractValidator<EndTurnCommand>
{
    public EndTurnCommandValidator()
    {
        RuleFor(x => x.CharacterId).NotEmpty();
    }
}
