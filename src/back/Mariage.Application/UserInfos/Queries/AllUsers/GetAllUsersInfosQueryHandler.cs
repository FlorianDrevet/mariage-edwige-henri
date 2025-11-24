using ErrorOr;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Domain.UserAggregate;
using MediatR;

namespace Mariage.Application.UserInfos.Queries.AllUsers;

public class GetAllUsersInfosQueryHandler(IUserRepository userRepository)
    : IRequestHandler<GetAllUsersInfosQuery, ErrorOr<List<User>>>
{
    public async Task<ErrorOr<List<User>>> Handle(GetAllUsersInfosQuery query, CancellationToken cancellationToken)
    {
        return userRepository.GetAllUsers();
    }
}