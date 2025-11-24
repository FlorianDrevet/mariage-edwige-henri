using Mariage.Domain.GiftAggregate;
using MediatR;
using ErrorOr;
using Mariage.Domain.GiftAggregate.ValueObjects;

namespace Mariage.Application.Gifts.Queries.GetGiftById;

public record GetGiftByIdQuery(
    GiftId GiftId
    ): IRequest<ErrorOr<Gift>>;