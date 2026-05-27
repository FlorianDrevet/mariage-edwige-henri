using MapsterMapper;
using Mariage.Api.Errors;
using Mariage.Application.Authentication.Commands.RefreshToken;
using Mariage.Application.Authentication.Commands.Register;
using Mariage.Application.Authentication.Queries.Login;
using Mariage.Contracts.Authentication;
using MediatR;

namespace Mariage.Api.Controllers;

public static class AuthenticationController
{
    public static IApplicationBuilder UseAuthenticationController(this IApplicationBuilder builder)
    {
        return builder.UseEndpoints(endpoints =>
        {
            endpoints.MapPost("/auth/register",
                    async (RegisterRequest request, IMediator mediator, IMapper mapper) =>
                    {
                        var command = mapper.Map<RegisterCommand>(request);
                        var authenticationResult = await mediator.Send(command);

                        return authenticationResult.Match(
                            authenticationResult =>
                            {
                                var user = mapper.Map<AuthenticationResponse>(authenticationResult);
                                return Results.Ok(user);
                            },
                            error => error.Result());
                    })
                .WithName("Register")
                .RequireAuthorization("IsAdmin")
                .WithOpenApi();

            endpoints.MapPost("/auth/login",
                    async (LoginRequest request, IMediator mediator, IMapper mapper) =>
                    {
                        var query = mapper.Map<LoginQuery>(request);
                        var authenticationResult = await mediator.Send(query);

                        return authenticationResult.Match(
                            authenticationResult =>
                            {
                                var user = mapper.Map<AuthenticationResponse>(authenticationResult);
                                return Results.Ok(user);
                            },
                            error => error.Result()
                        );
                    })
                .WithName("Login")
                .RequireRateLimiting("Login")
                .WithOpenApi();

            endpoints.MapPost("/auth/refresh",
                    async (RefreshTokenRequest request, IMediator mediator, IMapper mapper) =>
                    {
                        var command = new RefreshTokenCommand(request.Token, request.RefreshToken);
                        var result = await mediator.Send(command);

                        return result.Match(
                            authResult =>
                            {
                                var response = mapper.Map<AuthenticationResponse>(authResult);
                                return Results.Ok(response);
                            },
                            error => error.Result()
                        );
                    })
                .WithName("RefreshToken")
                .WithOpenApi();
        });
    }
}