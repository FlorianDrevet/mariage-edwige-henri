namespace Mariage.Application.Pictures.Common;

public record PictureResult(
    Guid Id,
    bool IsFavorite,
    string UrlImage,
    string Username);