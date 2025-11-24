using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Domain.UserAggregate;
using MediatR;
using ErrorOr;

namespace Mariage.Application.UserInfos.Commands;

public class ChangeEmailCommandHandler(IUserRepository userRepository)
    : IRequestHandler<ChangeEmailCommand, ErrorOr<User>>
{
    public async Task<ErrorOr<User>> Handle(ChangeEmailCommand request, CancellationToken cancellationToken)
    {
        var user = userRepository.GetUserById(request.UserId);
        user.ChangeEmail(request.Email);
        userRepository.UpdateUser(user);
        return user;
    }
}