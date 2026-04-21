using ErrorOr;
using Mariage.Domain.GiftAggregate.ValueObjects;
using MediatR;

namespace Mariage.Application.Gifts.Commands.DeleteGiftCategory;

public record DeleteGiftCategoryCommand(
    GiftCategoryId GiftCategoryId
) : IRequest<ErrorOr<Deleted>>;
