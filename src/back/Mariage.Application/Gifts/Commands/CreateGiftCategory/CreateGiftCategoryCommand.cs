using ErrorOr;
using Mariage.Domain.GiftAggregate;
using MediatR;

namespace Mariage.Application.Gifts.Commands.CreateGiftCategory;

public record CreateGiftCategoryCommand(
    string Name
) : IRequest<ErrorOr<GiftCategory>>;
