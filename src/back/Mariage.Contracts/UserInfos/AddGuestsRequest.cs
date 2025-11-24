using Mariage.Domain.UserAggregate.ValueObjects;

namespace Mariage.Contracts.UserInfos;

public record AddGuestsRequest(
    Guid UserId,
    List<GestDto> Guests);
    
public record GestDto(
    string FirstName,
    string LastName
);