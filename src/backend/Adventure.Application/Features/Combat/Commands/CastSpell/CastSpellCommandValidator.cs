using FluentValidation;

namespace Adventure.Application.Features.Combat.Commands.CastSpell;

public class CastSpellCommandValidator : AbstractValidator<CastSpellCommand>
{
    public CastSpellCommandValidator()
    {
        RuleFor(x => x.CharacterId).NotEmpty();
        RuleFor(x => x.SpellId).NotEmpty();
    }
}
