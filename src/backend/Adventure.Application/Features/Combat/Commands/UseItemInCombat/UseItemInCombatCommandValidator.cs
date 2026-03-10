using FluentValidation;

namespace Adventure.Application.Features.Combat.Commands.UseItemInCombat;

public class UseItemInCombatCommandValidator : AbstractValidator<UseItemInCombatCommand>
{
    public UseItemInCombatCommandValidator()
    {
        RuleFor(x => x.CharacterId).NotEmpty();
        RuleFor(x => x.ItemId).NotEmpty();
    }
}
