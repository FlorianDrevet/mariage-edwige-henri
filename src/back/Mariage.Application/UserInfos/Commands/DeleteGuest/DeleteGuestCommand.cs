using ErrorOr;
using Mariage.Domain.UserAggregate;
using Mariage.Domain.UserAggregate.ValueObjects;
using MediatR;

namespace Mariage.Application.UserInfos.Commands.DeleteGuest;

public record DeleteGuestCommand(
    UserId UserId,
    GuestId GuestId
) : IRequest<ErrorOr<User>>;
