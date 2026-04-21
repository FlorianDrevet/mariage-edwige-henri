using ErrorOr;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Domain.AccommodationAggregate;
using Mariage.Domain.Common.Errors;
using MediatR;

namespace Mariage.Application.Accommodations.Commands.UnassignAccommodation;

public class UnassignAccommodationCommandHandler(
    IAccommodationRepository accommodationRepository
) : IRequestHandler<UnassignAccommodationCommand, ErrorOr<Accommodation>>
{
    public async Task<ErrorOr<Accommodation>> Handle(
        UnassignAccommodationCommand request,
        CancellationToken cancellationToken)
    {
        var accommodation = await accommodationRepository.GetByIdAsync(request.AccommodationId);
        if (accommodation is null)
            return Errors.Accommodation.NotFound();

        accommodation.UnassignUser(request.UserId);
        await accommodationRepository.UpdateAsync(accommodation);
        return accommodation;
    }
}
