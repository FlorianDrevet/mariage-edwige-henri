using ErrorOr;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Domain.AccommodationAggregate;
using Mariage.Domain.Common.Errors;
using MediatR;

namespace Mariage.Application.Accommodations.Commands.CancelBooking;

public class CancelBookingCommandHandler(
    IAccommodationRepository accommodationRepository
) : IRequestHandler<CancelBookingCommand, ErrorOr<Accommodation>>
{
    public async Task<ErrorOr<Accommodation>> Handle(
        CancelBookingCommand request,
        CancellationToken cancellationToken)
    {
        var accommodation = await accommodationRepository.GetByIdAsync(request.AccommodationId);
        if (accommodation is null)
            return Errors.Accommodation.NotFound();

        var result = accommodation.CancelBooking(request.BookingId);
        if (result.IsError)
            return result.Errors;

        await accommodationRepository.UpdateAsync(accommodation);
        return accommodation;
    }
}
