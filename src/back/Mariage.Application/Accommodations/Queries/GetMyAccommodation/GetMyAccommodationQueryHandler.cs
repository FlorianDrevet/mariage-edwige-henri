using ErrorOr;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Domain.AccommodationAggregate;
using Mariage.Domain.Common.Errors;
using MediatR;

namespace Mariage.Application.Accommodations.Queries.GetMyAccommodation;

public class GetMyAccommodationQueryHandler(
    IAccommodationRepository accommodationRepository
) : IRequestHandler<GetMyAccommodationQuery, ErrorOr<Accommodation>>
{
    public async Task<ErrorOr<Accommodation>> Handle(
        GetMyAccommodationQuery request,
        CancellationToken cancellationToken)
    {
        var accommodation = await accommodationRepository.GetByUserIdAsync(request.UserId);
        if (accommodation is null)
            return Errors.Accommodation.UserNotAssigned();

        return accommodation;
    }
}
