using ErrorOr;
using Mariage.Domain.AccommodationAggregate;
using Mariage.Domain.AccommodationAggregate.ValueObjects;
using MediatR;

namespace Mariage.Application.Accommodations.Commands.UpdateAccommodation;

public record UpdateAccommodationCommand(
    AccommodationId AccommodationId,
    string Title,
    string Description,
    string UrlImage,
    decimal Price
) : IRequest<ErrorOr<Accommodation>>;
