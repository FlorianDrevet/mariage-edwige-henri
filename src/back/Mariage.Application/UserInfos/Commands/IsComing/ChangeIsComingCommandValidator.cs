using FluentValidation;

namespace Mariage.Application.UserInfos.Commands.IsComing;

public class ChangeIsComingCommandValidator: AbstractValidator<ChangeIsComingCommand>
{
    public ChangeIsComingCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}