using Microsoft.AspNetCore.Http;

namespace Mariage.Contracts.Accommodation;

public record UpdateAccommodationRequest(
    string Title,
    string Description,
    decimal PricePerPersonPerNight,
    int NumberOfNights,
    int Capacity,
    IFormFile? ImageFile);
