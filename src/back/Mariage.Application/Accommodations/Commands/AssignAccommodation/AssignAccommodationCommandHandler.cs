using ErrorOr;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Domain.AccommodationAggregate;
using Mariage.Domain.Common.Errors;
using MediatR;

namespace Mariage.Application.Accommodations.Commands.AssignAccommodation;

public class AssignAccommodationCommandHandler(
    IAccommodationRepository accommodationRepository
) : IRequestHandler<AssignAccommodationCommand, ErrorOr<Accommodation>>
{
    public async Task<ErrorOr<Accommodation>> Handle(
        AssignAccommodationCommand request,
        CancellationToken cancellationToken)
    {
        var accommodation = await accommodationRepository.GetByIdAsync(request.AccommodationId);
        if (accommodation is null)
            return Errors.Accommodation.NotFound();

        foreach (var userId in request.UserIds)
        {
            var existingAccommodation = await accommodationRepository.GetByUserIdAsync(userId);
            if (existingAccommodation is not null && existingAccommodation.Id != accommodation.Id)
                return Errors.Accommodation.AlreadyAssignedElsewhere();

            accommodation.AssignUser(userId);
        }

        await accommodationRepository.UpdateAsync(accommodation);
        return accommodation;
    }
}
