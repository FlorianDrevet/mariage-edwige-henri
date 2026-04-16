using FluentValidation;

namespace Mariage.Application.Gifts.Commands.UpdateGift;

public class UpdateGiftCommandValidator : AbstractValidator<UpdateGiftCommand>
{
    public UpdateGiftCommandValidator()
    {
        RuleFor(x => x.GiftId).NotNull();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Price).NotEmpty();
    }
}
