using FluentValidation;

namespace Mariage.Application.Accommodations.Commands.AssignAccommodation;

public class AssignAccommodationCommandValidator : AbstractValidator<AssignAccommodationCommand>
{
    public AssignAccommodationCommandValidator()
    {
        RuleFor(x => x.AccommodationId)
            .NotNull()
            .WithMessage("Accommodation ID is required.");

        RuleFor(x => x.UserIds)
            .NotEmpty()
            .WithMessage("At least one user ID is required.");
    }
}
