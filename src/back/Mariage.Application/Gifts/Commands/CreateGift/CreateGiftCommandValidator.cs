using FluentValidation;

namespace Mariage.Application.Gifts.Commands.CreateGift;

public class CreateGiftCommandValidator: AbstractValidator<CreateGiftCommand>
{
    public CreateGiftCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Price).NotEmpty();
    }
}