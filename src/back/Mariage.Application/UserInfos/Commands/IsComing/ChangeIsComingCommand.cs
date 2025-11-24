using ErrorOr;
using Mariage.Domain.UserAggregate;
using Mariage.Domain.UserAggregate.ValueObjects;
using MediatR;

namespace Mariage.Application.UserInfos.Commands.IsComing;

public record ChangeIsComingCommand(bool IsComing, UserId UserId, GuestId GuestId):IRequest<ErrorOr<User>>;