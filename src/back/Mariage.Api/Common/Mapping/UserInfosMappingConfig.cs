using Azure.Core;
using Mapster;
using Mariage.Application.Authentication.Common;
using Mariage.Application.Pictures.Commands.CreatePicture;
using Mariage.Application.Pictures.Common;
using Mariage.Application.UserInfos.Commands;
using Mariage.Application.UserInfos.Commands.AddGuests;
using Mariage.Application.UserInfos.Commands.IsComing;
using Mariage.Contracts.Authentication;
using Mariage.Contracts.Pictures;
using Mariage.Contracts.UserInfos;
using Mariage.Domain.PictureAggregate;
using Mariage.Domain.UserAggregate;
using Mariage.Domain.UserAggregate.Entities;
using Mariage.Domain.UserAggregate.ValueObjects;

namespace Mariage.Api.Common.Mapping;

public class UserInfosMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(ChangeEmailRequest Request, UserId UserId), ChangeEmailCommand>()
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest.Email, src => src.Request.Email);
        
        config.NewConfig<(ChangeIsComingRequest Request, UserId UserId), ChangeIsComingCommand>()
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest.GuestId, src => new GuestId(Guid.Parse(src.Request.GuestId)))
            .Map(dest => dest.IsComing, src => src.Request.IsComing);

        config.NewConfig<AddGuestsRequest, AddGuestsCommand>()
            .Map(dest => dest.UserId.Value, src => src.UserId);
        
        config.NewConfig<Guest, GuestResponse>()
            .Map(dest => dest.Id, src => src.Id.Value);
        
        config.NewConfig<User, UserInfosResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Accommodation, src => (UserAccommodationResponse?)null);

        config.NewConfig<(User User, Domain.AccommodationAggregate.Accommodation? Accommodation), UserInfosResponse>()
            .Map(dest => dest.Id, src => src.User.Id.Value)
            .Map(dest => dest.Username, src => src.User.Username)
            .Map(dest => dest.Email, src => src.User.Email)
            .Map(dest => dest.Guests, src => src.User.Guests)
            .Map(dest => dest.Accommodation, src => src.Accommodation != null
                ? new UserAccommodationResponse(
                    src.Accommodation.Id.Value,
                    src.Accommodation.Title,
                    src.Accommodation.Description,
                    src.Accommodation.UrlImage,
                    src.User.IsAccommodationAccepted)
                : null);
    }
}