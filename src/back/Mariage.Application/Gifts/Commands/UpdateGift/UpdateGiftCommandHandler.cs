using ErrorOr;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Domain.Common.Errors;
using Mariage.Domain.GiftAggregate;
using MediatR;

namespace Mariage.Application.Gifts.Commands.UpdateGift;

public class UpdateGiftCommandHandler(IGiftRepository giftRepository)
    : IRequestHandler<UpdateGiftCommand, ErrorOr<Gift>>
{
    public async Task<ErrorOr<Gift>> Handle(
        UpdateGiftCommand request,
        CancellationToken cancellationToken)
    {
        var gift = giftRepository.GetGiftById(request.GiftId);
        if (gift is null)
        {
            return Errors.Gift.GiftNotFound();
        }

        gift.Update(request.Name, request.Price, request.UrlImage, request.Category);
        giftRepository.UpdateGift(gift);
        return gift;
    }
}
