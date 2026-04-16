using ErrorOr;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Application.Common.Interfaces.Services;
using Mariage.Domain.Common.Errors;
using MediatR;

namespace Mariage.Application.Gifts.Commands.DeleteGift;

public class DeleteGiftCommandHandler(
    IGiftRepository giftRepository,
    IBlobService blobService)
    : IRequestHandler<DeleteGiftCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> Handle(
        DeleteGiftCommand request,
        CancellationToken cancellationToken)
    {
        var gift = giftRepository.GetGiftById(request.GiftId);
        if (gift is null)
        {
            return Errors.Gift.GiftNotFound();
        }

        if (!string.IsNullOrEmpty(gift.UrlImage))
        {
            var fileName = Path.GetFileName(new Uri(gift.UrlImage).LocalPath);
            await blobService.DeleteFileAsync(fileName);
        }

        giftRepository.DeleteGift(gift);
        return Result.Deleted;
    }
}
