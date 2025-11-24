namespace Mariage.Contracts.UserInfos;

public record UserInfosResponse(
    Guid Id,
    string Username,
    string Email,
    List<GuestResponse> Guests);
    
public record GuestResponse(
    Guid Id,
    string FirstName,
    string LastName,
    bool IsComing);