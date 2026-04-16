using ErrorOr;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Domain.Common.Errors;
using Mariage.Domain.UserAggregate;
using MediatR;

namespace Mariage.Application.UserInfos.Commands.UpdateGuest;

public class UpdateGuestCommandHandler(IUserRepository userRepository)
    : IRequestHandler<UpdateGuestCommand, ErrorOr<User>>
{
    public async Task<ErrorOr<User>> Handle(
        UpdateGuestCommand request,
        CancellationToken cancellationToken)
    {
        var user = userRepository.GetUserById(request.UserId);
        if (user is null)
        {
            return Errors.User.NotFoundUserWithIdError();
        }

        if (!user.UpdateGuest(request.GuestId, request.FirstName, request.LastName))
        {
            return Error.NotFound("Guest.NotFound", "The guest was not found.");
        }

        userRepository.UpdateUser(user);
        return user;
    }
}
