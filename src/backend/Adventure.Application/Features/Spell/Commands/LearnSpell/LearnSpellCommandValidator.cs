using FluentValidation;

namespace Adventure.Application.Features.Spell.Commands.LearnSpell;

public class LearnSpellCommandValidator : AbstractValidator<LearnSpellCommand>
{
    public LearnSpellCommandValidator()
    {
        RuleFor(x => x.CharacterId).NotEmpty();
        RuleFor(x => x.SpellId).NotEmpty();
    }
}
