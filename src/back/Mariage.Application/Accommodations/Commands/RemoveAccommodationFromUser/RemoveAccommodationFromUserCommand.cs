using ErrorOr;
using Mariage.Domain.UserAggregate;
using Mariage.Domain.UserAggregate.ValueObjects;
using MediatR;

namespace Mariage.Application.Accommodations.Commands.RemoveAccommodationFromUser;

public record RemoveAccommodationFromUserCommand(
    UserId UserId
) : IRequest<ErrorOr<User>>;
