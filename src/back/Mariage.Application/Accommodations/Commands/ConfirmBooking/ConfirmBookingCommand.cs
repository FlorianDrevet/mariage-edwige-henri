using ErrorOr;
using Mariage.Domain.AccommodationAggregate;
using Mariage.Domain.AccommodationAggregate.ValueObjects;
using Mariage.Domain.UserAggregate.ValueObjects;
using MediatR;

namespace Mariage.Application.Accommodations.Commands.ConfirmBooking;

public record ConfirmBookingCommand(
    AccommodationId AccommodationId,
    AccommodationBookingId BookingId,
    UserId UserId
) : IRequest<ErrorOr<Accommodation>>;
