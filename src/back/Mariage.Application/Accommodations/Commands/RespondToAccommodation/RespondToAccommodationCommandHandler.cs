using ErrorOr;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Domain.Common.Errors;
using Mariage.Domain.UserAggregate;
using MediatR;

namespace Mariage.Application.Accommodations.Commands.RespondToAccommodation;

public class RespondToAccommodationCommandHandler(
    IUserRepository userRepository,
    IAccommodationRepository accommodationRepository)
    : IRequestHandler<RespondToAccommodationCommand, ErrorOr<User>>
{
    public async Task<ErrorOr<User>> Handle(
        RespondToAccommodationCommand request,
        CancellationToken cancellationToken)
    {
        var user = userRepository.GetUserById(request.UserId);
        if (user is null)
        {
            return Errors.User.NotFoundUserWithIdError();
        }

        if (user.AccommodationId is null)
        {
            return Errors.Accommodation.NotFound();
        }

        var accommodation = accommodationRepository.GetById(user.AccommodationId);
        if (accommodation is null)
        {
            return Errors.Accommodation.NotFound();
        }

        user.RespondToAccommodation(request.Accepted);
        userRepository.UpdateUser(user);
        return user;
    }
}
