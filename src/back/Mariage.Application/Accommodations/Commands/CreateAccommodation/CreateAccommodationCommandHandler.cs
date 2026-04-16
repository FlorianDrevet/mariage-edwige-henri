using ErrorOr;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Domain.AccommodationAggregate;
using MediatR;

namespace Mariage.Application.Accommodations.Commands.CreateAccommodation;

public class CreateAccommodationCommandHandler(IAccommodationRepository accommodationRepository)
    : IRequestHandler<CreateAccommodationCommand, ErrorOr<Accommodation>>
{
    public async Task<ErrorOr<Accommodation>> Handle(
        CreateAccommodationCommand request,
        CancellationToken cancellationToken)
    {
        var accommodation = Accommodation.Create(request.Title, request.Description, request.UrlImage);
        accommodationRepository.Add(accommodation);
        return accommodation;
    }
}
