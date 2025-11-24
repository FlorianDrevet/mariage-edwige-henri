using ErrorOr;
using Mariage.Domain.UserAggregate;
using Mariage.Domain.UserAggregate.ValueObjects;
using MediatR;

namespace Mariage.Application.UserInfos.Commands;

public record ChangeEmailCommand(string Email, UserId UserId): IRequest<ErrorOr<User>>;
