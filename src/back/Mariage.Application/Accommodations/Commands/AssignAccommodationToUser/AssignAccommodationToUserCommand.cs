using ErrorOr;
using Mariage.Domain.AccommodationAggregate.ValueObjects;
using Mariage.Domain.UserAggregate;
using Mariage.Domain.UserAggregate.ValueObjects;
using MediatR;

namespace Mariage.Application.Accommodations.Commands.AssignAccommodationToUser;

public record AssignAccommodationToUserCommand(
    UserId UserId,
    AccommodationId AccommodationId
) : IRequest<ErrorOr<User>>;
