using ErrorOr;
using Mariage.Domain.GiftAggregate;
using MediatR;

namespace Mariage.Application.Gifts.Queries.GetGifts;

public record GetGiftQuery(): IRequest<ErrorOr<List<Gift>>>;