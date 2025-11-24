namespace Mariage.Api.Controllers;

public static class HealthCheckController
{
    public static IApplicationBuilder UseWakingUpController(this IApplicationBuilder builder)
    {
        return builder.UseEndpoints(endpoints =>
        {
            endpoints.MapPost("/healthz",
                    () => Results.Ok())
                .WithName("WakingUp Backend when free plan")
                .WithOpenApi();
        });
    }
}