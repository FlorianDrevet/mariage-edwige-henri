using ErrorOr;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Domain.Common.Errors;
using Mariage.Domain.UserAggregate;
using MediatR;

namespace Mariage.Application.UserInfos.Commands.DeleteGuest;

public class DeleteGuestCommandHandler(IUserRepository userRepository)
    : IRequestHandler<DeleteGuestCommand, ErrorOr<User>>
{
    public async Task<ErrorOr<User>> Handle(
        DeleteGuestCommand request,
        CancellationToken cancellationToken)
    {
        var user = userRepository.GetUserById(request.UserId);
        if (user is null)
        {
            return Errors.User.NotFoundUserWithIdError();
        }

        if (!user.RemoveGuest(request.GuestId))
        {
            return Error.NotFound("Guest.NotFound", "The guest was not found.");
        }

        userRepository.UpdateUser(user);
        return user;
    }
}
