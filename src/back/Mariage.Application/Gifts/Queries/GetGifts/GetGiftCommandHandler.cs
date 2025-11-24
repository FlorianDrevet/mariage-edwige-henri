using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Domain.GiftAggregate;
using MediatR;
using ErrorOr;

namespace Mariage.Application.Gifts.Queries.GetGifts;

public class GetGiftCommandHandler(IGiftRepository giftRepository)
    : IRequestHandler<GetGiftQuery, ErrorOr<List<Gift>>>
{
    public Task<ErrorOr<List<Gift>>> Handle(GetGiftQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(giftRepository.GetGifts());
    }
}