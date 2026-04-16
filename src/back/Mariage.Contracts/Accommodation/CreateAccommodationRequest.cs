namespace Mariage.Contracts.Accommodation;

public record CreateAccommodationRequest(
    string Title,
    string Description,
    string UrlImage);
