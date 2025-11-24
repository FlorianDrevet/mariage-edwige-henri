using FluentValidation;

namespace Mariage.Application.Gifts.Commands.CreateGiftParticipation;

public class CreateGiftParticipationCommandValidator: AbstractValidator<CreateGiftParticipationCommand>
{
    public CreateGiftParticipationCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Amount).NotEmpty().GreaterThan(0);
    }
}