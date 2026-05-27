using Mapster;

namespace Mariage.Api.Common.Mapping;

public class AccommodationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Manual mapping is done in AccommodationController — no Mapster config needed
    }
}

