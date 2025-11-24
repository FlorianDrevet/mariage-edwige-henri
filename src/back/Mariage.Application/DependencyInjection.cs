using System.Reflection;
using ErrorOr;
using FluentValidation;
using Mariage.Application.Authentication.Commands.Register;
using Mariage.Application.Common.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Mariage.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // CQRS with MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly, Assembly.GetExecutingAssembly()));
        
        // Behaviors
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        
        // Validators
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        return services;
    }
}