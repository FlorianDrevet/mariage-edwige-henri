using Mapster;
using Mariage.Application.Accommodations.Commands.AssignAccommodationToUser;
using Mariage.Application.Accommodations.Commands.CreateAccommodation;
using Mariage.Application.Accommodations.Commands.RemoveAccommodationFromUser;
using Mariage.Application.Accommodations.Commands.UpdateAccommodation;
using Mariage.Contracts.Accommodation;
using Mariage.Domain.AccommodationAggregate;
using Mariage.Domain.AccommodationAggregate.ValueObjects;
using Mariage.Domain.UserAggregate;
using Mariage.Domain.UserAggregate.ValueObjects;

namespace Mariage.Api.Common.Mapping;

public class AccommodationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateAccommodationRequest, CreateAccommodationCommand>();

        config.NewConfig<(UpdateAccommodationRequest Request, Guid AccommodationId), UpdateAccommodationCommand>()
            .Map(dest => dest.AccommodationId, src => AccommodationId.Create(src.AccommodationId))
            .Map(dest => dest.Title, src => src.Request.Title)
            .Map(dest => dest.Description, src => src.Request.Description)
            .Map(dest => dest.UrlImage, src => src.Request.UrlImage);

        config.NewConfig<Accommodation, AccommodationResponse>()
            .Map(dest => dest.Id, src => src.Id.Value);

        config.NewConfig<AssignAccommodationRequest, AssignAccommodationToUserCommand>()
            .Map(dest => dest.UserId, src => UserId.Create(src.UserId))
            .Map(dest => dest.AccommodationId, src => AccommodationId.Create(src.AccommodationId));
    }
}
