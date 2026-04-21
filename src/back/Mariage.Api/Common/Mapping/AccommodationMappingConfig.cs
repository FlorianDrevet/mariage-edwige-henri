using Mapster;
using Mariage.Application.Accommodations.Commands.CreateAccommodation;
using Mariage.Application.Accommodations.Commands.UpdateAccommodation;
using Mariage.Contracts.Accommodation;
using Mariage.Domain.AccommodationAggregate;
using Mariage.Domain.AccommodationAggregate.ValueObjects;

namespace Mariage.Api.Common.Mapping;

public class AccommodationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(CreateAccommodationRequest Request, string urlImage), CreateAccommodationCommand>()
            .Map(dest => dest.Title, src => src.Request.Title)
            .Map(dest => dest.Description, src => src.Request.Description)
            .Map(dest => dest.UrlImage, src => src.urlImage)
            .Map(dest => dest.Price, src => src.Request.Price);

        config.NewConfig<(UpdateAccommodationRequest Request, string urlImage, AccommodationId AccommodationId), UpdateAccommodationCommand>()
            .Map(dest => dest.AccommodationId, src => src.AccommodationId)
            .Map(dest => dest.Title, src => src.Request.Title)
            .Map(dest => dest.Description, src => src.Request.Description)
            .Map(dest => dest.UrlImage, src => src.urlImage)
            .Map(dest => dest.Price, src => src.Request.Price);

        config.NewConfig<Accommodation, AccommodationResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Price, src => src.Price)
            .Map(dest => dest.Assignments, src => src.Assignments);

        config.NewConfig<Mariage.Domain.AccommodationAggregate.Entities.AccommodationAssignment, AccommodationAssignmentResponse>()
            .Map(dest => dest.UserId, src => src.UserId.Value)
            .Map(dest => dest.Username, src => "")
            .Map(dest => dest.ResponseStatus, src => src.ResponseStatus.ToString());
    }
}
