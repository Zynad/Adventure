using FluentValidation;

namespace Adventure.Application.Features.Combat.Commands.ExecuteAttack;

public class ExecuteAttackCommandValidator : AbstractValidator<ExecuteAttackCommand>
{
    public ExecuteAttackCommandValidator()
    {
        RuleFor(x => x.CharacterId).NotEmpty();
        RuleFor(x => x.TargetId).NotEmpty();
    }
}
