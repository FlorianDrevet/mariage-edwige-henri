using Microsoft.AspNetCore.Diagnostics;

namespace Mariage.Api.Errors;

public static class ErrorHandling
{
    public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
    {
        return builder.UseExceptionHandler(exceptionHandlerApp
            => exceptionHandlerApp.Run(async context
                    =>
                {
                    var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

                    if (exception is not null)
                    {
                        var logger = context.RequestServices
                            .GetRequiredService<ILoggerFactory>()
                            .CreateLogger("GlobalExceptionHandler");

                        logger.LogError(exception,
                            "Unhandled exception on {Method} {Path}",
                            context.Request.Method,
                            context.Request.Path);
                    }

                    var (statusCode, message) = exception switch
                    {
                        _ => (StatusCodes.Status500InternalServerError, "An error occurred.")
                    };
                    await Results.Problem(
                            statusCode: statusCode,
                            detail: message
                        )
                        .ExecuteAsync(context);
                }
            )
        );
    }
}