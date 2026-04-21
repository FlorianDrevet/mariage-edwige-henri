using ErrorOr;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Domain.AccommodationAggregate;
using Mariage.Domain.Common.Errors;
using MediatR;

namespace Mariage.Application.Accommodations.Commands.RespondToAccommodation;

public class RespondToAccommodationCommandHandler(
    IAccommodationRepository accommodationRepository
) : IRequestHandler<RespondToAccommodationCommand, ErrorOr<Accommodation>>
{
    public async Task<ErrorOr<Accommodation>> Handle(
        RespondToAccommodationCommand request,
        CancellationToken cancellationToken)
    {
        var accommodation = await accommodationRepository.GetByUserIdAsync(request.UserId);
        if (accommodation is null)
            return Errors.Accommodation.UserNotAssigned();

        accommodation.SetUserResponse(request.UserId, request.Response);
        await accommodationRepository.UpdateAsync(accommodation);
        return accommodation;
    }
}
