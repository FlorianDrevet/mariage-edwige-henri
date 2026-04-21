using ErrorOr;
using Mariage.Domain.AccommodationAggregate;
using Mariage.Domain.AccommodationAggregate.ValueObjects;
using Mariage.Domain.UserAggregate.ValueObjects;
using MediatR;

namespace Mariage.Application.Accommodations.Commands.UnassignAccommodation;

public record UnassignAccommodationCommand(
    AccommodationId AccommodationId,
    UserId UserId
) : IRequest<ErrorOr<Accommodation>>;
