using FluentValidation;

namespace Mariage.Application.Accommodations.Commands.CreateAccommodation;

public class CreateAccommodationCommandValidator : AbstractValidator<CreateAccommodationCommand>
{
    public CreateAccommodationCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.");

        RuleFor(x => x.UrlImage)
            .NotEmpty()
            .WithMessage("Image URL is required.");
    }
}
