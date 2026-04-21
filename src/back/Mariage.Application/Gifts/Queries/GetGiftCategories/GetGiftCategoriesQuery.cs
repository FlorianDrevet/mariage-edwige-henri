using ErrorOr;
using Mariage.Domain.GiftAggregate;
using MediatR;

namespace Mariage.Application.Gifts.Queries.GetGiftCategories;

public record GetGiftCategoriesQuery() : IRequest<ErrorOr<List<GiftCategory>>>;
