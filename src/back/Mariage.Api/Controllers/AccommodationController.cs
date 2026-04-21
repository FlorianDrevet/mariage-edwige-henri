using System.Security.Claims;
using MapsterMapper;
using Mariage.Api.Errors;
using Mariage.Application.Accommodations.Commands.AssignAccommodation;
using Mariage.Application.Accommodations.Commands.CreateAccommodation;
using Mariage.Application.Accommodations.Commands.DeleteAccommodation;
using Mariage.Application.Accommodations.Commands.RespondToAccommodation;
using Mariage.Application.Accommodations.Commands.UnassignAccommodation;
using Mariage.Application.Accommodations.Commands.UpdateAccommodation;
using Mariage.Application.Accommodations.Queries.GetAccommodations;
using Mariage.Application.Accommodations.Queries.GetMyAccommodation;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Application.Common.Interfaces.Services;
using Mariage.Contracts.Accommodation;
using Mariage.Domain.AccommodationAggregate.Enums;
using Mariage.Domain.AccommodationAggregate.ValueObjects;
using Mariage.Domain.UserAggregate.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mariage.Api.Controllers;

public static class AccommodationController
{
    public static IApplicationBuilder UseAccommodationController(this IApplicationBuilder builder)
    {
        return builder.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("/accommodations",
                    async (IMediator mediator, IMapper mapper, IUserRepository userRepository) =>
                    {
                        var query = new GetAccommodationsQuery();
                        var result = await mediator.Send(query);

                        return result.Match(
                            accommodations =>
                            {
                                var users = userRepository.GetAllUsers();
                                var userMap = users.ToDictionary(u => u.Id.Value, u => u.Username);
                                
                                var response = accommodations.Select(a => new AccommodationResponse(
                                    a.Id.Value,
                                    a.Title,
                                    a.Description,
                                    a.UrlImage,
                                    a.Price,
                                    a.Assignments.Select(assign => new AccommodationAssignmentResponse(
                                        assign.UserId.Value,
                                        userMap.GetValueOrDefault(assign.UserId.Value, "Unknown"),
                                        assign.ResponseStatus.ToString()
                                    )).ToList()
                                )).ToList();
                                return Results.Ok(response);
                            },
                            error => error.Result());
                    })
                .RequireAuthorization("IsAdmin")
                .WithName("GetAccommodations")
                .WithOpenApi();

            endpoints.MapPost("/accommodations",
                    async (
                        IMediator mediator,
                        IMapper mapper,
                        IBlobService blobService,
                        [FromForm] CreateAccommodationRequest request) =>
                    {
                        if (request.ImageFile.Length == 0)
                        {
                            return Results.BadRequest("Image file is required.");
                        }

                        var fileName = Path.GetFileName(request.ImageFile.FileName);
                        await using var stream = request.ImageFile.OpenReadStream();
                        var imageUrl = await blobService.UploadFileAsync(stream, fileName);

                        var command = mapper.Map<CreateAccommodationCommand>((request, imageUrl));
                        var result = await mediator.Send(command);

                        return result.Match(
                            accommodation =>
                            {
                                var response = mapper.Map<AccommodationResponse>(accommodation);
                                return Results.Ok(response);
                            },
                            error => error.Result());
                    })
                .RequireAuthorization("IsAdmin")
                .WithName("CreateAccommodation")
                .DisableAntiforgery()
                .WithOpenApi();

            endpoints.MapPut("/accommodations/{id}",
                    async (
                        IMediator mediator,
                        IMapper mapper,
                        IBlobService blobService,
                        [FromForm] UpdateAccommodationRequest request,
                        Guid id) =>
                    {
                        string? imageUrl = null;
                        if (request.ImageFile is { Length: > 0 })
                        {
                            var fileName = Path.GetFileName(request.ImageFile.FileName);
                            await using var stream = request.ImageFile.OpenReadStream();
                            imageUrl = await blobService.UploadFileAsync(stream, fileName);
                        }

                        var existing = await mediator.Send(new GetAccommodationsQuery());
                        var currentImageUrl = existing.IsError
                            ? ""
                            : existing.Value.FirstOrDefault(a => a.Id == AccommodationId.Create(id))?.UrlImage ?? "";
                        var finalImageUrl = imageUrl ?? currentImageUrl;

                        var command = mapper.Map<UpdateAccommodationCommand>((request, finalImageUrl, AccommodationId.Create(id)));
                        var result = await mediator.Send(command);

                        return result.Match(
                            accommodation =>
                            {
                                var response = mapper.Map<AccommodationResponse>(accommodation);
                                return Results.Ok(response);
                            },
                            error => error.Result());
                    })
                .RequireAuthorization("IsAdmin")
                .WithName("UpdateAccommodation")
                .DisableAntiforgery()
                .WithOpenApi();

            endpoints.MapDelete("/accommodations/{id}",
                    async (IMediator mediator, Guid id) =>
                    {
                        var command = new DeleteAccommodationCommand(AccommodationId.Create(id));
                        var result = await mediator.Send(command);

                        return result.Match(
                            _ => Results.NoContent(),
                            error => error.Result());
                    })
                .RequireAuthorization("IsAdmin")
                .WithName("DeleteAccommodation")
                .WithOpenApi();

            endpoints.MapPost("/accommodations/{id}/assignments",
                    async (IMediator mediator, IMapper mapper, AssignAccommodationRequest request, Guid id) =>
                    {
                        var command = new AssignAccommodationCommand(
                            AccommodationId.Create(id),
                            request.UserIds.Select(uid => UserId.Create(uid)).ToList());
                        var result = await mediator.Send(command);

                        return result.Match(
                            accommodation =>
                            {
                                var response = mapper.Map<AccommodationResponse>(accommodation);
                                return Results.Ok(response);
                            },
                            error => error.Result());
                    })
                .RequireAuthorization("IsAdmin")
                .WithName("AssignAccommodation")
                .WithOpenApi();

            endpoints.MapDelete("/accommodations/{id}/assignments/{userId}",
                    async (IMediator mediator, IMapper mapper, Guid id, Guid userId) =>
                    {
                        var command = new UnassignAccommodationCommand(
                            AccommodationId.Create(id),
                            UserId.Create(userId));
                        var result = await mediator.Send(command);

                        return result.Match(
                            accommodation =>
                            {
                                var response = mapper.Map<AccommodationResponse>(accommodation);
                                return Results.Ok(response);
                            },
                            error => error.Result());
                    })
                .RequireAuthorization("IsAdmin")
                .WithName("UnassignAccommodation")
                .WithOpenApi();

            endpoints.MapGet("/accommodations/my",
                    async (IMediator mediator, IMapper mapper, HttpContext httpContext) =>
                    {
                        var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                        if (userIdClaim == null)
                        {
                            return Results.BadRequest("User ID claim not found in token.");
                        }

                        var query = new GetMyAccommodationQuery(UserId.Create(Guid.Parse(userIdClaim)));
                        var result = await mediator.Send(query);

                        return result.Match(
                            accommodation =>
                            {
                                var assignment = accommodation.Assignments
                                    .FirstOrDefault(a => a.UserId == UserId.Create(Guid.Parse(userIdClaim)));
                                var response = new MyAccommodationResponse(
                                    accommodation.Id.Value,
                                    accommodation.Title,
                                    accommodation.Description,
                                    accommodation.UrlImage,
                                    accommodation.Price,
                                    assignment?.ResponseStatus.ToString() ?? "Pending");
                                return Results.Ok(response);
                            },
                            error => error.Result());
                    })
                .RequireAuthorization()
                .WithName("GetMyAccommodation")
                .WithOpenApi();

            endpoints.MapPut("/accommodations/my/response",
                    async (IMediator mediator, HttpContext httpContext, RespondToAccommodationRequest request) =>
                    {
                        var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                        if (userIdClaim == null)
                        {
                            return Results.BadRequest("User ID claim not found in token.");
                        }

                        if (!Enum.TryParse<AccommodationResponseStatus>(request.Response, true, out var status))
                        {
                            return Results.BadRequest("Invalid response. Must be 'Accepted' or 'Refused'.");
                        }

                        var command = new RespondToAccommodationCommand(
                            UserId.Create(Guid.Parse(userIdClaim)),
                            status);
                        var result = await mediator.Send(command);

                        return result.Match(
                            accommodation =>
                            {
                                var assignment = accommodation.Assignments
                                    .FirstOrDefault(a => a.UserId == UserId.Create(Guid.Parse(userIdClaim)));
                                var response = new MyAccommodationResponse(
                                    accommodation.Id.Value,
                                    accommodation.Title,
                                    accommodation.Description,
                                    accommodation.UrlImage,
                                    accommodation.Price,
                                    assignment?.ResponseStatus.ToString() ?? "Pending");
                                return Results.Ok(response);
                            },
                            error => error.Result());
                    })
                .RequireAuthorization()
                .WithName("RespondToAccommodation")
                .WithOpenApi();
        });
    }
}
