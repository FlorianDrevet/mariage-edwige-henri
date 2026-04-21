using ErrorOr;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Domain.Common.Errors;
using MediatR;

namespace Mariage.Application.Accommodations.Commands.DeleteAccommodation;

public class DeleteAccommodationCommandHandler(
    IAccommodationRepository accommodationRepository
) : IRequestHandler<DeleteAccommodationCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> Handle(
        DeleteAccommodationCommand request,
        CancellationToken cancellationToken)
    {
        var accommodation = await accommodationRepository.GetByIdAsync(request.AccommodationId);
        if (accommodation is null)
            return Errors.Accommodation.NotFound();

        await accommodationRepository.DeleteAsync(accommodation);
        return Result.Deleted;
    }
}
