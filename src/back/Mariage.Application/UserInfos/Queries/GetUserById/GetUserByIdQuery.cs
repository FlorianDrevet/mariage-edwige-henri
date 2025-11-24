using Mariage.Domain.UserAggregate;
using Mariage.Domain.UserAggregate.ValueObjects;
using MediatR;
using ErrorOr;

namespace Mariage.Application.UserInfos.Queries.GetUserById;

public record GetUserByIdQuery(UserId UserId):IRequest<ErrorOr<User>>;