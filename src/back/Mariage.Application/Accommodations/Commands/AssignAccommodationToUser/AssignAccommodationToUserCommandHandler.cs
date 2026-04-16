using ErrorOr;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Domain.Common.Errors;
using Mariage.Domain.UserAggregate;
using MediatR;

namespace Mariage.Application.Accommodations.Commands.AssignAccommodationToUser;

public class AssignAccommodationToUserCommandHandler(
    IUserRepository userRepository,
    IAccommodationRepository accommodationRepository)
    : IRequestHandler<AssignAccommodationToUserCommand, ErrorOr<User>>
{
    public async Task<ErrorOr<User>> Handle(
        AssignAccommodationToUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = userRepository.GetUserById(request.UserId);
        if (user is null)
        {
            return Errors.User.NotFoundUserWithIdError();
        }

        var accommodation = accommodationRepository.GetById(request.AccommodationId);
        if (accommodation is null)
        {
            return Errors.Accommodation.NotFound();
        }

        user.AssignAccommodation(request.AccommodationId);
        userRepository.UpdateUser(user);
        return user;
    }
}
