using ErrorOr;
using Mariage.Domain.UserAggregate;
using MediatR;

namespace Mariage.Application.UserInfos.Queries.AllUsers;

public record GetAllUsersInfosQuery(): IRequest<ErrorOr<List<User>>>;