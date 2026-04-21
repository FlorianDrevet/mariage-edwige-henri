using Microsoft.AspNetCore.Http;

namespace Mariage.Contracts.Accommodation;

public record CreateAccommodationRequest(
    string Title,
    string Description,
    decimal Price,
    IFormFile ImageFile);
