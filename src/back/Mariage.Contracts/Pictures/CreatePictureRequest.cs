using Microsoft.AspNetCore.Http;

namespace Mariage.Contracts.Pictures;

public record CreatePictureRequest(
    IFormFile ImageFile);