using ErrorOr;
using Mariage.Domain.AccommodationAggregate;
using MediatR;

namespace Mariage.Application.Accommodations.Commands.CreateAccommodation;

public record CreateAccommodationCommand(
    string Title,
    string Description,
    string UrlImage,
    decimal Price
) : IRequest<ErrorOr<Accommodation>>;
