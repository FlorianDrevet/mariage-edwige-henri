using ErrorOr;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Domain.AccommodationAggregate;
using MediatR;

namespace Mariage.Application.Accommodations.Queries.GetAllAccommodations;

public class GetAllAccommodationsQueryHandler(IAccommodationRepository accommodationRepository)
    : IRequestHandler<GetAllAccommodationsQuery, ErrorOr<List<Accommodation>>>
{
    public async Task<ErrorOr<List<Accommodation>>> Handle(
        GetAllAccommodationsQuery request,
        CancellationToken cancellationToken)
    {
        return accommodationRepository.GetAll();
    }
}
