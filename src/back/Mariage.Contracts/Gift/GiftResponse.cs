namespace Mariage.Contracts.Gift;

public record GiftResponse(
    Guid Id,
    string Name,
    float Price,
    float Participation,
    string UrlImage,
    string Category,
    List<GiftGiverResponse> GiftGivers);

public record GiftGiverResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    float Amount);
