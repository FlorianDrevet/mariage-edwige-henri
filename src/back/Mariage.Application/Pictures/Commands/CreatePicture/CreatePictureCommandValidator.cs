using FluentValidation;

namespace Mariage.Application.Pictures.Commands.CreatePicture;

public class CreatePictureCommandValidator: AbstractValidator<CreatePictureCommand>
{
    public CreatePictureCommandValidator()
    {
        RuleFor(x => x.UrlPicture).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}