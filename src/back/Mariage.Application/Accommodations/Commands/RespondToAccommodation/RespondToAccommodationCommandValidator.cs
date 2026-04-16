using FluentValidation;

namespace Mariage.Application.Accommodations.Commands.RespondToAccommodation;

public class RespondToAccommodationCommandValidator : AbstractValidator<RespondToAccommodationCommand>
{
    public RespondToAccommodationCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotNull()
            .WithMessage("User ID is required.");
    }
}
