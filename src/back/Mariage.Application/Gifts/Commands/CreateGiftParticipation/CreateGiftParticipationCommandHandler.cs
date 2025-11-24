using ErrorOr;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Application.Common.Interfaces.Services;
using Mariage.Application.Gifts.Commands.CreateGift;
using Mariage.Domain.Common.Errors;
using Mariage.Domain.GiftAggregate;
using Mariage.Domain.GiftAggregate.Entities;
using MediatR;

namespace Mariage.Application.Gifts.Commands.CreateGiftParticipation;

public class CreateGiftParticipationCommandHandler(IGiftRepository giftRepository, IDiscordWebhook discordWebhook): 
    IRequestHandler<CreateGiftParticipationCommand, ErrorOr<Gift>>
{
    public async Task<ErrorOr<Gift>> Handle(
        CreateGiftParticipationCommand request,
        CancellationToken cancellationToken)
    {
        if (giftRepository.GetGiftById(request.GiftId) is not Gift gift)
        {
            return Errors.Gift.GiftNotFound();
        }

        if (request.Amount > gift.Price - gift.Participation)
        {
            return Errors.Participation.AmountExceedParticipationLeft();
        }
        
        var giftGiver = GiftGiver.Create(request.FirstName, request.LastName, request.Email, request.Amount);
        var giftResponse = giftRepository.AddGiftGiver(request.GiftId, giftGiver);
        await discordWebhook.SendDiscordWebhook(
            $"New participation for {gift.Name} from {request.FirstName} {request.LastName} with {request.Amount}€");
        return giftResponse;
    }
}