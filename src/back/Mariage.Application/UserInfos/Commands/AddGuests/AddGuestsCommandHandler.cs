using ErrorOr;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Domain.Common.Errors;
using Mariage.Domain.UserAggregate;
using Mariage.Domain.UserAggregate.Entities;
using MediatR;

namespace Mariage.Application.UserInfos.Commands.AddGuests;

public class AddGuestsCommandHandler(IUserRepository userRepository):
    IRequestHandler<AddGuestsCommand, ErrorOr<User>>
{
    public async Task<ErrorOr<User>> Handle(AddGuestsCommand request, CancellationToken cancellationToken)
    {
        var user = userRepository.GetUserById(request.UserId);
        if (user is null)
        {
            return Errors.User.NotFoundUserWithIdError();
        }

        var guests = request
            .Guests
            .Select(guest => Guest.Create(guest.FirstName, guest.LastName)).ToList();
        return userRepository.AddGuests(request.UserId, guests);
    }
}