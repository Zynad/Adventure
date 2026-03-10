using Adventure.Domain.Enums;
using FluentValidation;

namespace Adventure.Application.Features.Combat.Commands.ExecuteCombatAction;

public class ExecuteCombatActionCommandValidator : AbstractValidator<ExecuteCombatActionCommand>
{
    public ExecuteCombatActionCommandValidator()
    {
        RuleFor(x => x.CharacterId).NotEmpty();

        RuleFor(x => x.ActionType)
            .IsInEnum()
            .Must(a => a != CombatActionType.CastSpell && a != CombatActionType.Dash)
            .WithMessage("CastSpell and Dash are not yet available.");

        RuleFor(x => x.TargetId)
            .NotEmpty()
            .When(x => x.ActionType is CombatActionType.Attack or CombatActionType.Help)
            .WithMessage("TargetId is required for Attack and Help actions.");
    }
}
