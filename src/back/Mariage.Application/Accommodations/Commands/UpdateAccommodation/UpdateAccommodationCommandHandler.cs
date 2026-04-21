using ErrorOr;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Domain.AccommodationAggregate;
using Mariage.Domain.Common.Errors;
using MediatR;

namespace Mariage.Application.Accommodations.Commands.UpdateAccommodation;

public class UpdateAccommodationCommandHandler(
    IAccommodationRepository accommodationRepository
) : IRequestHandler<UpdateAccommodationCommand, ErrorOr<Accommodation>>
{
    public async Task<ErrorOr<Accommodation>> Handle(
        UpdateAccommodationCommand request,
        CancellationToken cancellationToken)
    {
        var accommodation = await accommodationRepository.GetByIdAsync(request.AccommodationId);
        if (accommodation is null)
            return Errors.Accommodation.NotFound();

        accommodation.Update(request.Title, request.Description, request.UrlImage, request.Price);
        await accommodationRepository.UpdateAsync(accommodation);
        return accommodation;
    }
}
