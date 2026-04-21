using Microsoft.AspNetCore.Http;

namespace Mariage.Contracts.Accommodation;

public record UpdateAccommodationRequest(
    string Title,
    string Description,
    decimal Price,
    IFormFile? ImageFile);
