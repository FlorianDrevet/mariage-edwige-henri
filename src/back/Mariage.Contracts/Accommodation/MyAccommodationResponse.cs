namespace Mariage.Contracts.Accommodation;

public record MyAccommodationResponse(
    Guid Id,
    string Title,
    string Description,
    string UrlImage,
    decimal Price,
    string ResponseStatus);
