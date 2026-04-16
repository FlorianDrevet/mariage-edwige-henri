using ErrorOr;
using Mariage.Domain.UserAggregate;
using Mariage.Domain.UserAggregate.ValueObjects;
using MediatR;

namespace Mariage.Application.Accommodations.Commands.RespondToAccommodation;

public record RespondToAccommodationCommand(
    UserId UserId,
    bool Accepted
) : IRequest<ErrorOr<User>>;
