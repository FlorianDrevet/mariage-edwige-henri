using Azure.Core;
using Mapster;
using Mariage.Application.Gifts.Commands.CreateGift;
using Mariage.Application.Gifts.Commands.CreateGiftParticipation;
using Mariage.Contracts.Gift;
using Mariage.Domain.GiftAggregate;
using Mariage.Domain.GiftAggregate.Entities;
using Mariage.Domain.GiftAggregate.ValueObjects;

namespace Mariage.Api.Common.Mapping;

public class GiftMappingConfig: IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(CreateGiftRequest Request, string urlImage), CreateGiftCommand>()
            .Map(dest => dest.Name, src => src.Request.Name)
            .Map(dest => dest.Price, src => src.Request.Price)
            .Map(dest => dest.Category, src => new GiftCategory(src.Request.Category))
            .Map(dest => dest.UrlImage, src => src.urlImage);

        config.NewConfig<Gift, GiftResponse>()
            .Map(dest => dest.Category, src => (int)src.Category.Value)
            .Map(dest => dest.Id, src => src.Id.Value);

        config.NewConfig<GiftGiver, GiftGiverResponse>()
            .Map(dest => dest.Id, src => src.Id.Value);

        config.NewConfig<(CreateGiftParticipationRequest Request, Guid GiftId), CreateGiftParticipationCommand>()
            .Map(dest => dest.GiftId,
                src => new GiftId(src.GiftId))
            .Map(dest => dest, src => src.Request);
    }
}