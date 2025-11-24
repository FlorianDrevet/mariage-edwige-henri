using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Domain.GiftAggregate;
using MediatR;
using ErrorOr;
using Mariage.Domain.Common.Errors;

namespace Mariage.Application.Gifts.Queries.GetGiftById;

public class GetGiftByIdQueryHandler(IGiftRepository giftRepository)
    : IRequestHandler<GetGiftByIdQuery, ErrorOr<Gift>>
{
    public async Task<ErrorOr<Gift>> Handle(GetGiftByIdQuery request, CancellationToken cancellationToken)
    {
        var gift = giftRepository.GetGiftById(request.GiftId);
        
        if (gift is null)
        {
            return Errors.Gift.GiftNotFound();
        }
        
        return gift;
    }
}