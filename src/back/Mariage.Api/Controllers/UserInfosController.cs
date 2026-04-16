using System.Security.Claims;
using ErrorOr;
using MapsterMapper;
using Mariage.Api.Errors;
using Mariage.Application.UserInfos.Commands;
using Mariage.Application.UserInfos.Commands.AddGuests;
using Mariage.Application.UserInfos.Commands.DeleteGuest;
using Mariage.Application.UserInfos.Commands.DeleteUser;
using Mariage.Application.UserInfos.Commands.IsComing;
using Mariage.Application.UserInfos.Commands.UpdateGuest;
using Mariage.Application.UserInfos.Queries.AllUsers;
using Mariage.Application.UserInfos.Queries.GetUserById;
using Mariage.Contracts.Pictures;
using Mariage.Contracts.UserInfos;
using Mariage.Domain.UserAggregate.ValueObjects;
using MediatR;

namespace Mariage.Api.Controllers;

public static class UserInfosController
{
    public static IApplicationBuilder UseUserInfosController(this IApplicationBuilder builder)
    {
        return builder.UseEndpoints(endpoints =>
        {
            endpoints.MapPut("/user-infos/email",
                    async (IMediator mediator, IMapper mapper, 
                        ChangeEmailRequest request, HttpContext httpContext) =>
                    {
                        var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                        if (userId == null)
                        {
                            return Results.BadRequest("User ID claim not found in token.");
                        }
                        
                        var command = mapper.Map<ChangeEmailCommand>((request, UserId.Create(Guid.Parse(userId))));
                        var changeEmailResult = await mediator.Send(command);

                        return changeEmailResult.Match(
                            changeEmailResult =>
                            {
                                var user = mapper.Map<UserInfosResponse>(changeEmailResult);
                                return Results.Ok(user);
                            },
                            error => error.Result());
                    })
                .WithName("ChangeEmail")
                .RequireAuthorization()
                .WithOpenApi();
            
            endpoints.MapPut("/user-infos/is-coming",
                    async (IMediator mediator, IMapper mapper, 
                        ChangeIsComingRequest request, HttpContext httpContext) =>
                    {
                        var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                        if (userId == null)
                        {
                            return Results.BadRequest("User ID claim not found in token.");
                        }
                        
                        var command = mapper.Map<ChangeIsComingCommand>((request, UserId.Create(Guid.Parse(userId))));
                        var changeIsComingResult = await mediator.Send(command);

                        return changeIsComingResult.Match(
                            changeIsComingResult =>
                            {
                                var user = mapper.Map<UserInfosResponse>(changeIsComingResult);
                                return Results.Ok(user);
                            },
                            error => error.Result());
                    })
                .WithName("ChangeIsComing")
                .RequireAuthorization()
                .WithOpenApi(); 
            
            endpoints.MapGet("/user-infos",
                    async (IMediator mediator, IMapper mapper) =>
                    {
                        var query = new GetAllUsersInfosQuery();
                        var getAllUsersInfosResult = await mediator.Send(query);
                        
                        return getAllUsersInfosResult.Match(
                            getAllUsersInfosResult =>
                            {
                                var usersInfos = mapper.Map<List<UserInfosResponse>>(getAllUsersInfosResult);
                                return Results.Ok(usersInfos);
                            },
                            error => error.Result()
                        );
                    })
                .WithName("GetUsersInfos")
                .RequireAuthorization("IsAdmin")
                .WithOpenApi();  
            
            endpoints.MapGet("/user-infos/profils",
                    async (IMediator mediator, IMapper mapper, HttpContext httpContext) =>
                    {
                        var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                        if (userId == null)
                        {
                            return Results.BadRequest("User ID claim not found in token.");
                        }
                        var query = new GetUserByIdQuery(UserId.Create(Guid.Parse(userId)));
                        var getUserByIdInfosResult = await mediator.Send(query);
                        Console.WriteLine(getUserByIdInfosResult);
                        
                        return getUserByIdInfosResult.Match(
                            getUserByIdInfosResult =>
                            {
                                var usersInfos = mapper.Map<UserInfosResponse>(getUserByIdInfosResult);
                                return Results.Ok(usersInfos);
                            },
                            error => error.Result()
                        );
                    })
                .WithName("GetUserByIdInfos")
                .RequireAuthorization()
                .WithOpenApi();  
            
            endpoints.MapPost("/user-infos/guests",
                    async (IMediator mediator, IMapper mapper, 
                        AddGuestsRequest request) =>
                    {
                        Console.WriteLine(request.Guests.Count);
                        var command = mapper.Map<AddGuestsCommand>(request);
                        //Console.WriteLine(command.Guests.Count);
                        var addGuestsResult = await mediator.Send(command);

                        return addGuestsResult.Match(
                            addGuestsResult =>
                            {
                                var user = mapper.Map<UserInfosResponse>(addGuestsResult);
                                return Results.Ok(user);
                            },
                            error => error.Result());
                    })
                .WithName("AddGuests")
                .RequireAuthorization("IsAdmin")
                .WithOpenApi();

            endpoints.MapDelete("/user-infos/{userId}",
                    async (IMediator mediator, Guid userId) =>
                    {
                        var command = new DeleteUserCommand(UserId.Create(userId));
                        var deleteUserResult = await mediator.Send(command);

                        return deleteUserResult.Match(
                            _ => Results.NoContent(),
                            error => error.Result()
                        );
                    })
                .WithName("DeleteUser")
                .RequireAuthorization("IsAdmin")
                .WithOpenApi();

            endpoints.MapPut("/user-infos/{userId}/guests/{guestId}",
                    async (IMediator mediator, IMapper mapper,
                        UpdateGuestRequest request, Guid userId, Guid guestId) =>
                    {
                        var command = new UpdateGuestCommand(
                            UserId.Create(userId),
                            new GuestId(guestId),
                            request.FirstName,
                            request.LastName);
                        var updateGuestResult = await mediator.Send(command);

                        return updateGuestResult.Match(
                            result =>
                            {
                                var user = mapper.Map<UserInfosResponse>(result);
                                return Results.Ok(user);
                            },
                            error => error.Result()
                        );
                    })
                .WithName("UpdateGuest")
                .RequireAuthorization("IsAdmin")
                .WithOpenApi();

            endpoints.MapDelete("/user-infos/{userId}/guests/{guestId}",
                    async (IMediator mediator, IMapper mapper, Guid userId, Guid guestId) =>
                    {
                        var command = new DeleteGuestCommand(
                            UserId.Create(userId),
                            new GuestId(guestId));
                        var deleteGuestResult = await mediator.Send(command);

                        return deleteGuestResult.Match(
                            result =>
                            {
                                var user = mapper.Map<UserInfosResponse>(result);
                                return Results.Ok(user);
                            },
                            error => error.Result()
                        );
                    })
                .WithName("DeleteGuest")
                .RequireAuthorization("IsAdmin")
                .WithOpenApi();
        });
    }
}