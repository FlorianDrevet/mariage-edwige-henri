using Microsoft.AspNetCore.Http;

namespace Mariage.Contracts.Gift;

public record UpdateGiftRequest(
    string Name,
    float Price,
    IFormFile? ImageFile,
    string Category
);
