using FluentValidation;

namespace Mariage.Application.UserInfos.Commands.UpdateGuest;

public class UpdateGuestCommandValidator : AbstractValidator<UpdateGuestCommand>
{
    public UpdateGuestCommandValidator()
    {
        RuleFor(x => x.UserId).NotNull();
        RuleFor(x => x.GuestId).NotNull();
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
    }
}
