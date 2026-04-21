using FluentValidation;

namespace Mariage.Application.Accommodations.Commands.UpdateAccommodation;

public class UpdateAccommodationCommandValidator : AbstractValidator<UpdateAccommodationCommand>
{
    public UpdateAccommodationCommandValidator()
    {
        RuleFor(x => x.AccommodationId)
            .NotNull()
            .WithMessage("Accommodation ID is required.");

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
