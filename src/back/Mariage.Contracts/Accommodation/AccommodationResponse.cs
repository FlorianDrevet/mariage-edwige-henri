namespace Mariage.Contracts.Accommodation;

public record AccommodationResponse(
    Guid Id,
    string Title,
    string Description,
    string UrlImage,
    decimal PricePerPersonPerNight,
    int NumberOfNights,
    int Capacity,
    int RemainingCapacity,
    List<BookingResponse> Bookings);
