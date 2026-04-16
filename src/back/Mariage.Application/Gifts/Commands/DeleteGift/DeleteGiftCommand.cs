using ErrorOr;
using Mariage.Domain.GiftAggregate.ValueObjects;
using MediatR;

namespace Mariage.Application.Gifts.Commands.DeleteGift;

public record DeleteGiftCommand(
    GiftId GiftId
) : IRequest<ErrorOr<Deleted>>;
