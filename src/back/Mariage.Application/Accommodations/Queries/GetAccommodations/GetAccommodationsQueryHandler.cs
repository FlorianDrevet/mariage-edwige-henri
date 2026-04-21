using ErrorOr;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Domain.AccommodationAggregate;
using MediatR;

namespace Mariage.Application.Accommodations.Queries.GetAccommodations;

public class GetAccommodationsQueryHandler(
    IAccommodationRepository accommodationRepository,
    IUserRepository userRepository
) : IRequestHandler<GetAccommodationsQuery, ErrorOr<List<Accommodation>>>
{
    public async Task<ErrorOr<List<Accommodation>>> Handle(
        GetAccommodationsQuery request,
        CancellationToken cancellationToken)
    {
        var accommodations = await accommodationRepository.GetAllAsync();
        return accommodations;
    }
}
