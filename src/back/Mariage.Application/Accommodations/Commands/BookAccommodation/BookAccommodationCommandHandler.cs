using ErrorOr;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Application.Common.Interfaces.Services;
using Mariage.Domain.AccommodationAggregate;
using Mariage.Domain.Common.Errors;
using MediatR;

namespace Mariage.Application.Accommodations.Commands.BookAccommodation;

public class BookAccommodationCommandHandler(
    IAccommodationRepository accommodationRepository,
    IDiscordWebhook discordWebhook
) : IRequestHandler<BookAccommodationCommand, ErrorOr<Accommodation>>
{
    public async Task<ErrorOr<Accommodation>> Handle(
        BookAccommodationCommand request,
        CancellationToken cancellationToken)
    {
        var accommodation = await accommodationRepository.GetByIdAsync(request.AccommodationId);
        if (accommodation is null)
            return Errors.Accommodation.NotFound();

        var bookingResult = accommodation.Book(
            request.UserId,
            request.NumberOfPersons,
            request.FirstName,
            request.LastName,
            request.Email);

        if (bookingResult.IsError)
            return bookingResult.Errors;

        await accommodationRepository.UpdateAsync(accommodation);

        await discordWebhook.SendDiscordWebhook(
            $"🏨 Nouvelle réservation de {request.FirstName} {request.LastName} " +
            $"pour {request.NumberOfPersons} pers. à \"{accommodation.Title}\" " +
            $"— Total: {bookingResult.Value.TotalAmount}€");

        return accommodation;
    }
}
