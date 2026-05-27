using FluentValidation;

namespace Mariage.Application.Accommodations.Commands.BookAccommodation;

public class BookAccommodationCommandValidator : AbstractValidator<BookAccommodationCommand>
{
    public BookAccommodationCommandValidator()
    {
        RuleFor(x => x.NumberOfPersons).GreaterThan(0);
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
    }
}
