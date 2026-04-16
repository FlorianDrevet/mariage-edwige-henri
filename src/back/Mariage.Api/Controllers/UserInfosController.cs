using System.Security.Claims;
using ErrorOr;
using MapsterMapper;
using Mariage.Api.Errors;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Application.UserInfos.Commands;
using Mariage.Application.UserInfos.Commands.AddGuests;
using Mariage.Application.UserInfos.Commands.IsComing;
using Mariage.Application.UserInfos.Queries.AllUsers;
using Mariage.Application.UserInfos.Queries.GetUserById;
using Mariage.Contracts.Pictures;
using Mariage.Contracts.UserInfos;
using Mariage.Domain.UserAggregate;
using Mariage.Domain.UserAggregate.ValueObjects;
using MediatR;

namespace Mariage.Api.Controllers;

public static class UserInfosController
{
    internal static UserInfosResponse MapUserWithAccommodation(
        User user, IMapper mapper, IAccommodationRepository accommodationRepository)
    {
        var accommodation = user.AccommodationId is not null
            ? accommodationRepository.GetById(user.AccommodationId)
            : null;
        return mapper.Map<UserInfosResponse>((user, accommodation));
    }

    public static IApplicationBuilder UseUserInfosController(this IApplicationBuilder builder)
    {
        return builder.UseEndpoints(endpoints =>
        {
            endpoints.MapPut("/user-infos/email",
                    async (IMediator mediator, IMapper mapper, IAccommodationRepository accommodationRepository,
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
                            user =>
                            {
                                var response = MapUserWithAccommodation(user, mapper, accommodationRepository);
                                return Results.Ok(response);
                            },
                            error => error.Result());
                    })
                .WithName("ChangeEmail")
                .RequireAuthorization()
                .WithOpenApi();
            
            endpoints.MapPut("/user-infos/is-coming",
                    async (IMediator mediator, IMapper mapper, IAccommodationRepository accommodationRepository,
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
                            user =>
                            {
                                var response = MapUserWithAccommodation(user, mapper, accommodationRepository);
                                return Results.Ok(response);
                            },
                            error => error.Result());
                    })
                .WithName("ChangeIsComing")
                .RequireAuthorization()
                .WithOpenApi(); 
            
            endpoints.MapGet("/user-infos",
                    async (IMediator mediator, IMapper mapper, IAccommodationRepository accommodationRepository) =>
                    {
                        var query = new GetAllUsersInfosQuery();
                        var getAllUsersInfosResult = await mediator.Send(query);
                        
                        return getAllUsersInfosResult.Match(
                            users =>
                            {
                                var usersInfos = users
                                    .Select(u => MapUserWithAccommodation(u, mapper, accommodationRepository))
                                    .ToList();
                                return Results.Ok(usersInfos);
                            },
                            error => error.Result()
                        );
                    })
                .WithName("GetUsersInfos")
                .RequireAuthorization("IsAdmin")
                .WithOpenApi();  
            
            endpoints.MapGet("/user-infos/profils",
                    async (IMediator mediator, IMapper mapper, IAccommodationRepository accommodationRepository,
                        HttpContext httpContext) =>
                    {
                        var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                        if (userId == null)
                        {
                            return Results.BadRequest("User ID claim not found in token.");
                        }
                        var query = new GetUserByIdQuery(UserId.Create(Guid.Parse(userId)));
                        var getUserByIdInfosResult = await mediator.Send(query);
                        
                        return getUserByIdInfosResult.Match(
                            user =>
                            {
                                var response = MapUserWithAccommodation(user, mapper, accommodationRepository);
                                return Results.Ok(response);
                            },
                            error => error.Result()
                        );
                    })
                .WithName("GetUserByIdInfos")
                .RequireAuthorization()
                .WithOpenApi();  
            
            endpoints.MapPost("/user-infos/guests",
                    async (IMediator mediator, IMapper mapper, IAccommodationRepository accommodationRepository,
                        AddGuestsRequest request) =>
                    {
                        var command = mapper.Map<AddGuestsCommand>(request);
                        var addGuestsResult = await mediator.Send(command);

                        return addGuestsResult.Match(
                            user =>
                            {
                                var response = MapUserWithAccommodation(user, mapper, accommodationRepository);
                                return Results.Ok(response);
                            },
                            error => error.Result());
                    })
                .WithName("AddGuests")
                .RequireAuthorization("IsAdmin")
                .WithOpenApi(); 
        });
    }
}