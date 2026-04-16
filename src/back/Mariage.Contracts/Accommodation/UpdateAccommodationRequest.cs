namespace Mariage.Contracts.Accommodation;

public record UpdateAccommodationRequest(
    string Title,
    string Description,
    string UrlImage);
