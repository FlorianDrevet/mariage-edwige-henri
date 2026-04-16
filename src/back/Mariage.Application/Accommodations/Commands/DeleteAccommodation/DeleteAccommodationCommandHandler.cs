using ErrorOr;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Domain.Common.Errors;
using MediatR;

namespace Mariage.Application.Accommodations.Commands.DeleteAccommodation;

public class DeleteAccommodationCommandHandler(
    IAccommodationRepository accommodationRepository,
    IUserRepository userRepository)
    : IRequestHandler<DeleteAccommodationCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> Handle(
        DeleteAccommodationCommand request,
        CancellationToken cancellationToken)
    {
        var accommodation = accommodationRepository.GetById(request.AccommodationId);
        if (accommodation is null)
        {
            return Errors.Accommodation.NotFound();
        }

        // Remove accommodation assignment from users that have it
        var affectedUsers = userRepository.GetUsersByAccommodationId(request.AccommodationId);
        foreach (var user in affectedUsers)
        {
            user.RemoveAccommodation();
            userRepository.UpdateUser(user);
        }

        accommodationRepository.Delete(accommodation);
        return Result.Deleted;
    }
}
