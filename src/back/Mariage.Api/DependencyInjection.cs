using System.Reflection;
using Mariage.Api.Common.Mapping;
using MediatR;

namespace Mariage.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddMapping();
        services.AddAuthorization();
        return services;
    }
}