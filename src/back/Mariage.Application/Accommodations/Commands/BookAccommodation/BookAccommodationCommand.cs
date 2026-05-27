using ErrorOr;
using Mariage.Domain.AccommodationAggregate;
using Mariage.Domain.AccommodationAggregate.ValueObjects;
using Mariage.Domain.UserAggregate.ValueObjects;
using MediatR;

namespace Mariage.Application.Accommodations.Commands.BookAccommodation;

public record BookAccommodationCommand(
    AccommodationId AccommodationId,
    UserId UserId,
    int NumberOfPersons,
    string FirstName,
    string LastName,
    string? Email
) : IRequest<ErrorOr<Accommodation>>;
