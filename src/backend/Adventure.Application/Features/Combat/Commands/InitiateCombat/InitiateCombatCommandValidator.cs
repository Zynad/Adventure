using FluentValidation;

namespace Adventure.Application.Features.Combat.Commands.InitiateCombat;

public class InitiateCombatCommandValidator : AbstractValidator<InitiateCombatCommand>
{
    public InitiateCombatCommandValidator()
    {
        RuleFor(x => x.CharacterId).NotEmpty();
        RuleFor(x => x.MonsterIds).NotEmpty().WithMessage("At least one monster is required.");
        RuleForEach(x => x.MonsterIds).NotEmpty().WithMessage("Monster id must not be empty.");
    }
}
