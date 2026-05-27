using ErrorOr;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Domain.AccommodationAggregate;
using MediatR;

namespace Mariage.Application.Accommodations.Queries.GetMyBookings;

public class GetMyBookingsQueryHandler(
    IAccommodationRepository accommodationRepository
) : IRequestHandler<GetMyBookingsQuery, ErrorOr<List<Accommodation>>>
{
    public async Task<ErrorOr<List<Accommodation>>> Handle(
        GetMyBookingsQuery request,
        CancellationToken cancellationToken)
    {
        var accommodations = await accommodationRepository.GetByUserBookingsAsync(request.UserId);
        return accommodations;
    }
}
