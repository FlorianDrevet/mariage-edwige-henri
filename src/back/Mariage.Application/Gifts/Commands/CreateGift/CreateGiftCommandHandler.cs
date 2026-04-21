using ErrorOr;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Domain.GiftAggregate;
using MediatR;

namespace Mariage.Application.Gifts.Commands.CreateGift;

public class CreateGiftCommandHandler(IGiftRepository giftRepository): 
    IRequestHandler<CreateGiftCommand, ErrorOr<Domain.GiftAggregate.Gift>>
{
    public async Task<ErrorOr<Gift>> Handle(
        CreateGiftCommand request,
        CancellationToken cancellationToken)
    {
        var gift = Domain.GiftAggregate.Gift.Create(request.Name, request.Price, request.UrlImage, request.Category);
        giftRepository.AddGift(gift);
        return gift;
    }
}