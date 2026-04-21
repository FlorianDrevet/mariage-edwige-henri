using ErrorOr;
using Mariage.Domain.AccommodationAggregate;
using Mariage.Domain.AccommodationAggregate.ValueObjects;
using Mariage.Domain.UserAggregate.ValueObjects;
using MediatR;

namespace Mariage.Application.Accommodations.Commands.AssignAccommodation;

public record AssignAccommodationCommand(
    AccommodationId AccommodationId,
    List<UserId> UserIds
) : IRequest<ErrorOr<Accommodation>>;
