using ErrorOr;
using Mariage.Domain.AccommodationAggregate;
using Mariage.Domain.AccommodationAggregate.Enums;
using Mariage.Domain.UserAggregate.ValueObjects;
using MediatR;

namespace Mariage.Application.Accommodations.Commands.RespondToAccommodation;

public record RespondToAccommodationCommand(
    UserId UserId,
    AccommodationResponseStatus Response
) : IRequest<ErrorOr<Accommodation>>;
