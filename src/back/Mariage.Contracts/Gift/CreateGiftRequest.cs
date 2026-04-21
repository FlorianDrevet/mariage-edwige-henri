using Microsoft.AspNetCore.Http;

namespace Mariage.Contracts.Gift;

public record CreateGiftRequest(
    string Name,
    float Price,
    IFormFile ImageFile,
    string Category
);