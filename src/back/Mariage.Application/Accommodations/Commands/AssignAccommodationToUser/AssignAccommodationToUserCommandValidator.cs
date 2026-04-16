using FluentValidation;

namespace Mariage.Application.Accommodations.Commands.AssignAccommodationToUser;

public class AssignAccommodationToUserCommandValidator : AbstractValidator<AssignAccommodationToUserCommand>
{
    public AssignAccommodationToUserCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotNull()
            .WithMessage("User ID is required.");

        RuleFor(x => x.AccommodationId)
            .NotNull()
            .WithMessage("Accommodation ID is required.");
    }
}
