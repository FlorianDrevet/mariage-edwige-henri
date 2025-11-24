using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Domain.UserAggregate;
using ErrorOr;
using Mariage.Application.Common.Interfaces.Services;
using MediatR;

namespace Mariage.Application.UserInfos.Commands.IsComing;

public class ChangeIsComingCommandHandler(IUserRepository userRepository, IDiscordWebhook discordWebhook)
    : IRequestHandler<ChangeIsComingCommand, ErrorOr<User>>
{
    public async Task<ErrorOr<User>> Handle(ChangeIsComingCommand request, CancellationToken cancellationToken)
    {
        var user = userRepository.GetUserById(request.UserId);
        user!.ChangeIsComing(request.GuestId, request.IsComing);
        userRepository.UpdateUser(user);
        
        var guest = user.Guests.FirstOrDefault(x => x.Id == request.GuestId);
        if (guest is not null)
        {
            await discordWebhook.SendDiscordWebhook(
                $"{guest.FirstName} {guest.LastName} is {(request.IsComing ? "coming" : "not coming")} !");
        }
        
        return user;
    }
}