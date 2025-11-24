namespace Mariage.Contracts.Pictures;

public record PictureResponse(
    Guid Id,
    bool IsFavorite,
    string UrlImage,
    string Username);