using FluentValidation;

namespace Mariage.Application.UserInfos.Commands.AddGuests;

public class AddGuestsCommandValidator: AbstractValidator<AddGuestsCommand>
{
    public AddGuestsCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Guests).NotEmpty();
    }
}