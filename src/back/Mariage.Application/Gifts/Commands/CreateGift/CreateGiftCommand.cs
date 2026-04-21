using MediatR;
using ErrorOr;
using Mariage.Domain.GiftAggregate;

namespace Mariage.Application.Gifts.Commands.CreateGift;

public record CreateGiftCommand(
    string Name,
    float Price,
    string UrlImage,
    string Category
    ) : IRequest<ErrorOr<Gift>>;