using ErrorOr;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Domain.Common.Errors;
using MediatR;

namespace Mariage.Application.UserInfos.Commands.DeleteUser;

public class DeleteUserCommandHandler(IUserRepository userRepository)
    : IRequestHandler<DeleteUserCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> Handle(
        DeleteUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = userRepository.GetUserById(request.UserId);
        if (user is null)
        {
            return Errors.User.NotFoundUserWithIdError();
        }

        userRepository.DeleteUser(user);
        return Result.Deleted;
    }
}
