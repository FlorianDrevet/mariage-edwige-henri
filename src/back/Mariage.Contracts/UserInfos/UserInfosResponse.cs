namespace Mariage.Contracts.UserInfos;

public record UserInfosResponse(
    Guid Id,
    string Username,
    string Email,
    List<GuestResponse> Guests,
    UserAccommodationResponse? Accommodation);
    
public record GuestResponse(
    Guid Id,
    string FirstName,
    string LastName,
    bool IsComing);

public record UserAccommodationResponse(
    Guid Id,
    string Title,
    string Description,
    string UrlImage,
    bool? IsAccepted);