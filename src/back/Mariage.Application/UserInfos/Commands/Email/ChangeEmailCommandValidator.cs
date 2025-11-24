using FluentValidation;

namespace Mariage.Application.UserInfos.Commands.Email;

public class ChangeEmailCommandValidator: AbstractValidator<ChangeEmailCommand>
{
    public ChangeEmailCommandValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
    }
}