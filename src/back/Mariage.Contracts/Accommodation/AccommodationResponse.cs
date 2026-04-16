namespace Mariage.Contracts.Accommodation;

public record AccommodationResponse(
    Guid Id,
    string Title,
    string Description,
    string UrlImage);
