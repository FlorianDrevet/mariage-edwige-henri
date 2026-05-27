using ErrorOr;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Domain.AccommodationAggregate;
using Mariage.Domain.AccommodationAggregate.Enums;
using MediatR;

namespace Mariage.Application.Accommodations.Queries.GetAvailableAccommodations;

public class GetAvailableAccommodationsQueryHandler(
    IAccommodationRepository accommodationRepository
) : IRequestHandler<GetAvailableAccommodationsQuery, ErrorOr<List<Accommodation>>>
{
    public async Task<ErrorOr<List<Accommodation>>> Handle(
        GetAvailableAccommodationsQuery request,
        CancellationToken cancellationToken)
    {
        var accommodations = await accommodationRepository.GetAllAsync();
        return accommodations
            .Where(a => a.RemainingCapacity > 0)
            .ToList();
    }
}
