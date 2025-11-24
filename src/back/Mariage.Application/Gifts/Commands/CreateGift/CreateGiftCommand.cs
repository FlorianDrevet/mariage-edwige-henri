using MediatR;
using ErrorOr;
using Mariage.Domain.GiftAggregate;
using Mariage.Domain.GiftAggregate.ValueObjects;

namespace Mariage.Application.Gifts.Commands.CreateGift;

public record CreateGiftCommand(
    string Name,
    float Price,
    string UrlImage,
    GiftCategory Category
    ) : IRequest<ErrorOr<Gift>>;