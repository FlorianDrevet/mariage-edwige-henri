using ErrorOr;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Domain.GiftAggregate;
using MediatR;

namespace Mariage.Application.Gifts.Queries.GetGiftCategories;

public class GetGiftCategoriesQueryHandler(IGiftCategoryRepository giftCategoryRepository)
    : IRequestHandler<GetGiftCategoriesQuery, ErrorOr<List<GiftCategory>>>
{
    public async Task<ErrorOr<List<GiftCategory>>> Handle(
        GetGiftCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var categories = await giftCategoryRepository.GetAllAsync();
        return categories;
    }
}
