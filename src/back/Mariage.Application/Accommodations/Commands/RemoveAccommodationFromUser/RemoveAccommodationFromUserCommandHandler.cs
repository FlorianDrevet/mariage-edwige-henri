using ErrorOr;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Domain.Common.Errors;
using Mariage.Domain.UserAggregate;
using MediatR;

namespace Mariage.Application.Accommodations.Commands.RemoveAccommodationFromUser;

public class RemoveAccommodationFromUserCommandHandler(IUserRepository userRepository)
    : IRequestHandler<RemoveAccommodationFromUserCommand, ErrorOr<User>>
{
    public async Task<ErrorOr<User>> Handle(
        RemoveAccommodationFromUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = userRepository.GetUserById(request.UserId);
        if (user is null)
        {
            return Errors.User.NotFoundUserWithIdError();
        }

        user.RemoveAccommodation();
        userRepository.UpdateUser(user);
        return user;
    }
}
