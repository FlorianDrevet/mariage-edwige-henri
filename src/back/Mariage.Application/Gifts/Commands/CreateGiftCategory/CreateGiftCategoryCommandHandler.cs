using ErrorOr;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Domain.Common.Errors;
using Mariage.Domain.GiftAggregate;
using MediatR;

namespace Mariage.Application.Gifts.Commands.CreateGiftCategory;

public class CreateGiftCategoryCommandHandler(IGiftCategoryRepository giftCategoryRepository)
    : IRequestHandler<CreateGiftCategoryCommand, ErrorOr<GiftCategory>>
{
    public async Task<ErrorOr<GiftCategory>> Handle(
        CreateGiftCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var existing = await giftCategoryRepository.GetByNameAsync(request.Name);
        if (existing is not null)
        {
            return Errors.GiftCategory.GiftCategoryDuplicateName();
        }

        var category = GiftCategory.Create(request.Name);
        await giftCategoryRepository.AddAsync(category);
        return category;
    }
}
