using ErrorOr;
using Mariage.Domain.UserAggregate.ValueObjects;
using MediatR;

namespace Mariage.Application.UserInfos.Commands.DeleteUser;

public record DeleteUserCommand(
    UserId UserId
) : IRequest<ErrorOr<Deleted>>;
