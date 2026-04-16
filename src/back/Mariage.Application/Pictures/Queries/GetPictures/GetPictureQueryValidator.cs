using FluentValidation;

namespace Mariage.Application.Pictures.Queries;

public class GetPictureQueryValidator : AbstractValidator<GetPictureQuery>
{
    public GetPictureQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("PageNumber must be at least 1.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("PageSize must be between 1 and 100.");
    }
}