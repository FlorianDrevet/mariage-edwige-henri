using System.Security.Claims;
using MapsterMapper;
using Mariage.Api.Errors;
using Mariage.Application.Accommodations.Commands.AssignAccommodationToUser;
using Mariage.Application.Accommodations.Commands.CreateAccommodation;
using Mariage.Application.Accommodations.Commands.DeleteAccommodation;
using Mariage.Application.Accommodations.Commands.RemoveAccommodationFromUser;
using Mariage.Application.Accommodations.Commands.RespondToAccommodation;
using Mariage.Application.Accommodations.Commands.UpdateAccommodation;
using Mariage.Application.Accommodations.Queries.GetAllAccommodations;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Contracts.Accommodation;
using Mariage.Contracts.UserInfos;
using Mariage.Domain.AccommodationAggregate.ValueObjects;
using Mariage.Domain.UserAggregate.ValueObjects;
using MediatR;

namespace Mariage.Api.Controllers;

public static class AccommodationController
{
    public static IApplicationBuilder UseAccommodationController(this IApplicationBuilder builder)
    {
        return builder.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("/accommodations",
                    async (IMediator mediator, IMapper mapper) =>
                    {
                        var query = new GetAllAccommodationsQuery();
                        var result = await mediator.Send(query);

                        return result.Match(
                            accommodations =>
                            {
                                var response = mapper.Map<List<AccommodationResponse>>(accommodations);
                                return Results.Ok(response);
                            },
                            error => error.Result());
                    })
                .WithName("GetAllAccommodations")
                .RequireAuthorization("IsAdmin")
                .WithOpenApi();

            endpoints.MapPost("/accommodations",
                    async (IMediator mediator, IMapper mapper,
                        CreateAccommodationRequest request) =>
                    {
                        var command = mapper.Map<CreateAccommodationCommand>(request);
                        var result = await mediator.Send(command);

                        return result.Match(
                            accommodation =>
                            {
                                var response = mapper.Map<AccommodationResponse>(accommodation);
                                return Results.Ok(response);
                            },
                            error => error.Result());
                    })
                .WithName("CreateAccommodation")
                .RequireAuthorization("IsAdmin")
                .WithOpenApi();

            endpoints.MapPut("/accommodations/{accommodationId:guid}",
                    async (IMediator mediator, IMapper mapper,
                        Guid accommodationId, UpdateAccommodationRequest request) =>
                    {
                        var command = mapper.Map<UpdateAccommodationCommand>((request, accommodationId));
                        var result = await mediator.Send(command);

                        return result.Match(
                            accommodation =>
                            {
                                var response = mapper.Map<AccommodationResponse>(accommodation);
                                return Results.Ok(response);
                            },
                            error => error.Result());
                    })
                .WithName("UpdateAccommodation")
                .RequireAuthorization("IsAdmin")
                .WithOpenApi();

            endpoints.MapDelete("/accommodations/{accommodationId:guid}",
                    async (IMediator mediator, Guid accommodationId) =>
                    {
                        var command = new DeleteAccommodationCommand(AccommodationId.Create(accommodationId));
                        var result = await mediator.Send(command);

                        return result.Match(
                            _ => Results.NoContent(),
                            error => error.Result());
                    })
                .WithName("DeleteAccommodation")
                .RequireAuthorization("IsAdmin")
                .WithOpenApi();

            endpoints.MapPost("/accommodations/assign",
                    async (IMediator mediator, IMapper mapper, IAccommodationRepository accommodationRepository,
                        AssignAccommodationRequest request) =>
                    {
                        var command = mapper.Map<AssignAccommodationToUserCommand>(request);
                        var result = await mediator.Send(command);

                        return result.Match(
                            user =>
                            {
                                var response = UserInfosController.MapUserWithAccommodation(
                                    user, mapper, accommodationRepository);
                                return Results.Ok(response);
                            },
                            error => error.Result());
                    })
                .WithName("AssignAccommodationToUser")
                .RequireAuthorization("IsAdmin")
                .WithOpenApi();

            endpoints.MapDelete("/accommodations/assign/{userId:guid}",
                    async (IMediator mediator, IMapper mapper, IAccommodationRepository accommodationRepository,
                        Guid userId) =>
                    {
                        var command = new RemoveAccommodationFromUserCommand(UserId.Create(userId));
                        var result = await mediator.Send(command);

                        return result.Match(
                            user =>
                            {
                                var response = UserInfosController.MapUserWithAccommodation(
                                    user, mapper, accommodationRepository);
                                return Results.Ok(response);
                            },
                            error => error.Result());
                    })
                .WithName("RemoveAccommodationFromUser")
                .RequireAuthorization("IsAdmin")
                .WithOpenApi();

            endpoints.MapPut("/accommodations/respond",
                    async (IMediator mediator, IMapper mapper, IAccommodationRepository accommodationRepository,
                        RespondToAccommodationRequest request, HttpContext httpContext) =>
                    {
                        var userId = httpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                        if (userId == null)
                        {
                            return Results.BadRequest("User ID claim not found in token.");
                        }

                        var command = new RespondToAccommodationCommand(
                            UserId.Create(Guid.Parse(userId)),
                            request.Accepted);
                        var result = await mediator.Send(command);

                        return result.Match(
                            user =>
                            {
                                var response = UserInfosController.MapUserWithAccommodation(
                                    user, mapper, accommodationRepository);
                                return Results.Ok(response);
                            },
                            error => error.Result());
                    })
                .WithName("RespondToAccommodation")
                .RequireAuthorization()
                .WithOpenApi();
        });
    }
}
