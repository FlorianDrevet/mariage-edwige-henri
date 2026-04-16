using ErrorOr;
using Mariage.Domain.UserAggregate;
using Mariage.Domain.UserAggregate.ValueObjects;
using MediatR;

namespace Mariage.Application.UserInfos.Commands.UpdateGuest;

public record UpdateGuestCommand(
    UserId UserId,
    GuestId GuestId,
    string FirstName,
    string LastName
) : IRequest<ErrorOr<User>>;
