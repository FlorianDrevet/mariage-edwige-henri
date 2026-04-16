using ErrorOr;
using Mariage.Domain.GiftAggregate;
using Mariage.Domain.GiftAggregate.ValueObjects;
using MediatR;

namespace Mariage.Application.Gifts.Commands.UpdateGift;

public record UpdateGiftCommand(
    GiftId GiftId,
    string Name,
    float Price,
    string UrlImage,
    GiftCategory Category
) : IRequest<ErrorOr<Gift>>;
