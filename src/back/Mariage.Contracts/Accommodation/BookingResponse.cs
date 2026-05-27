namespace Mariage.Contracts.Accommodation;

public record BookingResponse(
    Guid Id,
    Guid AccommodationId,
    string AccommodationTitle,
    string AccommodationImage,
    int NumberOfPersons,
    decimal TotalAmount,
    string Status,
    string BookerFirstName,
    string BookerLastName,
    string? BookerEmail,
    DateTime CreatedAt);
