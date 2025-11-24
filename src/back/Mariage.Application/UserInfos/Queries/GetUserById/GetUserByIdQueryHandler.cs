using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Domain.UserAggregate;
using MediatR;
using ErrorOr;
using Mariage.Domain.Common.Errors;

namespace Mariage.Application.UserInfos.Queries.GetUserById;

public class GetUserByIdQueryHandler(IUserRepository userRepository) 
    : IRequestHandler<GetUserByIdQuery, ErrorOr<User>>
{
    public async Task<ErrorOr<User>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = userRepository.GetUserById(request.UserId);

        if (user is null)
        {
            return Errors.User.NotFoundUserWithIdError();
        }

        return user;
    }
}