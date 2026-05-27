using ErrorOr;
using Mariage.Domain.AccommodationAggregate;
using MediatR;

namespace Mariage.Application.Accommodations.Commands.CreateAccommodation;

public record CreateAccommodationCommand(
    string Title,
    string Description,
    string UrlImage,
    decimal PricePerPersonPerNight,
    int NumberOfNights,
    int Capacity
) : IRequest<ErrorOr<Accommodation>>;
