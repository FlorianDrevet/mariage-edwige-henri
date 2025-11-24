using System.Security.Claims;
using MapsterMapper;
using Mariage.Api.Errors;
using Mariage.Application.Common.Interfaces.Services;
using Mariage.Application.Pictures.Commands.AddPicturesToFavorites;
using Mariage.Application.Pictures.Commands.CreatePicture;
using Mariage.Application.Pictures.Commands.RemovePicture;
using Mariage.Application.Pictures.Commands.RemovePictureFromFavorites;
using Mariage.Application.Pictures.Queries;
using Mariage.Application.Pictures.Queries.GetFavoritesPictures;
using Mariage.Application.Pictures.Queries.GetPicturesTookByUser;
using Mariage.Contracts.Pictures;
using Mariage.Domain.PictureAggregate.ValueObject;
using Mariage.Domain.UserAggregate.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mariage.Api.Controllers;

public static class PicturesController
{
    public static IApplicationBuilder UsePicturesController(this IApplicationBuilder builder)
    {
        return builder.UseEndpoints(endpoints =>
        {
            endpoints.MapPost("/pictures",
                    async (
                        IMediator mediator,
                        IMapper mapper,
                        IBlobService blobService,
                        HttpContext httpContext,
                        [FromForm] CreatePictureRequest request) =>
                    {
                        var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                        if (userId == null)
                        {
                            return Results.BadRequest("User ID claim not found in token.");
                        }

                        if (request.ImageFile.Length == 0)
                        {
                            return Results.BadRequest("Image file is required.");
                        }

                        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.ImageFile.FileName)}";
                        await using var stream = request.ImageFile.OpenReadStream();
                        var imageUrl = await blobService.UploadPictureAsync(stream, fileName);

                        var command = mapper.Map<CreatePictureCommand>((imageUrl, UserId.Create(Guid.Parse(userId))));
                        var createPictureResult = await mediator.Send(command);

                        return createPictureResult.Match(
                            createPictureResult =>
                            {
                                var user = mapper.Map<PictureResponse>(createPictureResult);
                                return Results.Ok(user);
                            },
                            error => error.Result());
                    })
                .WithName("CreatePicture")
                .RequireAuthorization()
                .DisableAntiforgery()
                .WithOpenApi();

            endpoints.MapDelete("/pictures/{id}",
                    async (IMediator mediator, IMapper mapper, HttpContext httpContext, Guid id) =>
                    {
                        var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                        if (userId == null)
                        {
                            return Results.BadRequest("User ID claim not found in token.");
                        }

                        var command = new RemovePictureCommand(PictureId.Create(id), UserId.Create(Guid.Parse(userId)));
                        var removePictureResult = await mediator.Send(command);

                        return removePictureResult.Match(
                            _ => Results.NoContent(),
                            error => error.Result());
                    })
                .WithName("RemovePicture")
                .RequireAuthorization()
                .WithOpenApi();

            endpoints.MapGet("/pictures",
                    async (IMediator mediator, IMapper mapper, int page, int pageSize) =>
                    {
                        var query = new GetPictureQuery(page, pageSize);
                        var getPictureResult = await mediator.Send(query);

                        return getPictureResult.Match(
                            getPictureResult =>
                            {
                                var pictures = mapper.Map<List<PictureResponse>>(getPictureResult);
                                return Results.Ok(pictures);
                            },
                            error => error.Result()
                        );
                    })
                .WithName("GetPictures")
                .RequireAuthorization()
                .WithOpenApi();

            endpoints.MapGet("/pictures-photo-booth",
                    async (IMediator mediator, IMapper mapper) =>
                    {
                        var query = new GetPicturePhotoBoothQuery();
                        var getPictureResult = await mediator.Send(query);

                        return getPictureResult.Match(
                            getPictureResult => { return Results.Ok(getPictureResult); },
                            error => error.Result()
                        );
                    })
                .WithName("GetPicturesPhotoBooth")
                .RequireAuthorization()
                .WithOpenApi();

            endpoints.MapGet("/pictures-photograph",
                    async (IMediator mediator, IMapper mapper) =>
                    {
                        var query = new GetPicturePhotographQuery();
                        var getPictureResult = await mediator.Send(query);

                        return getPictureResult.Match(
                            getPictureResult => { return Results.Ok(getPictureResult); },
                            error => error.Result()
                        );
                    })
                .WithName("GetPicturesPhotograph")
                .RequireAuthorization()
                .WithOpenApi();

            endpoints.MapGet("/pictures/took-by-user",
                    async (IMediator mediator, IMapper mapper, HttpContext httpContext, int page, int pageSize) =>
                    {
                        var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                        if (userId == null)
                        {
                            return Results.BadRequest("User ID claim not found in token.");
                        }

                        var query = mapper.Map<GetPicturesTookByUserQuery>((page, pageSize, Guid.Parse(userId)));
                        var getPictureResult = await mediator.Send(query);

                        return getPictureResult.Match(
                            getPictureResult =>
                            {
                                var pictures = mapper.Map<List<PictureResponse>>(getPictureResult);
                                return Results.Ok(pictures);
                            },
                            error => error.Result()
                        );
                    })
                .WithName("GetPicturesTookByUserPaginated")
                .RequireAuthorization()
                .WithOpenApi();

            endpoints.MapGet("/pictures/favorites",
                    async (IMediator mediator, IMapper mapper, HttpContext httpContext, int page, int pageSize) =>
                    {
                        var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                        if (userId == null)
                        {
                            return Results.BadRequest("User ID claim not found in token.");
                        }

                        var query = mapper.Map<GetFavoritePicturesQuery>((page, pageSize, Guid.Parse(userId)));
                        var getPictureResult = await mediator.Send(query);

                        return getPictureResult.Match(
                            getPictureResult =>
                            {
                                var pictures = mapper.Map<List<PictureResponse>>(getPictureResult);
                                return Results.Ok(pictures);
                            },
                            error => error.Result()
                        );
                    })
                .WithName("GetFavoritePicturesPaginated")
                .RequireAuthorization()
                .WithOpenApi();

            endpoints.MapPost("/pictures/{pictureId}/favorites",
                    async (IMediator mediator, IMapper mapper, HttpContext httpContext, Guid pictureId) =>
                    {
                        var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                        if (userId == null)
                        {
                            return Results.BadRequest("User ID claim not found in token.");
                        }

                        var query = mapper.Map<AddPicturesToFavoritesCommand>((pictureId, Guid.Parse(userId)));
                        var getPictureResult = await mediator.Send(query);

                        return getPictureResult.Match(
                            _ => Results.Created(),
                            error => error.Result()
                        );
                    })
                .WithName("AddFavoritePictures")
                .RequireAuthorization()
                .WithOpenApi();

            endpoints.MapDelete("/pictures/{pictureId}/favorites",
                    async (IMediator mediator, IMapper mapper, HttpContext httpContext, Guid pictureId) =>
                    {
                        var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                        if (userId == null)
                        {
                            return Results.BadRequest("User ID claim not found in token.");
                        }

                        var query = mapper.Map<RemovePictureFromFavoritesCommand>((pictureId, Guid.Parse(userId)));
                        var getPictureResult = await mediator.Send(query);

                        return getPictureResult.Match(
                            _ => Results.NoContent(),
                            error => error.Result()
                        );
                    })
                .WithName("RemoveFavoritePictures")
                .RequireAuthorization()
                .WithOpenApi();
        });
    }
}