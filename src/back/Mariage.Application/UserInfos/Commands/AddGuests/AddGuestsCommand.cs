using ErrorOr;
using Mariage.Domain.UserAggregate;
using Mariage.Domain.UserAggregate.ValueObjects;
using MediatR;

namespace Mariage.Application.UserInfos.Commands.AddGuests;

public record AddGuestsCommand(
    List<GuestDto> Guests,
    UserId UserId
    ): IRequest<ErrorOr<User>>;
    
public record GuestDto(
    string FirstName,
    string LastName
    );