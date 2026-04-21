using FluentValidation;

namespace Mariage.Application.Gifts.Commands.CreateGiftCategory;

public class CreateGiftCategoryCommandValidator : AbstractValidator<CreateGiftCategoryCommand>
{
    public CreateGiftCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Category name is required.")
            .MaximumLength(100)
            .WithMessage("Category name must not exceed 100 characters.");
    }
}
