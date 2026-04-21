using ErrorOr;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Domain.Common.Errors;
using MediatR;

namespace Mariage.Application.Gifts.Commands.DeleteGiftCategory;

public class DeleteGiftCategoryCommandHandler(IGiftCategoryRepository giftCategoryRepository)
    : IRequestHandler<DeleteGiftCategoryCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> Handle(
        DeleteGiftCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var category = await giftCategoryRepository.GetByIdAsync(request.GiftCategoryId);
        if (category is null)
        {
            return Errors.GiftCategory.GiftCategoryNotFound();
        }

        var isUsed = await giftCategoryRepository.IsCategoryUsedByGiftsAsync(category.Name);
        if (isUsed)
        {
            return Errors.GiftCategory.GiftCategoryInUse();
        }

        await giftCategoryRepository.DeleteAsync(category);
        return Result.Deleted;
    }
}
