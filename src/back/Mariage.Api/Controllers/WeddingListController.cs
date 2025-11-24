using MapsterMapper;
using Mariage.Api.Errors;
using Mariage.Application.Common.Interfaces.Services;
using Mariage.Application.Gifts.Commands.CreateGift;
using Mariage.Application.Gifts.Commands.CreateGiftParticipation;
using Mariage.Application.Gifts.Queries.GetGiftById;
using Mariage.Application.Gifts.Queries.GetGifts;
using Mariage.Application.Pictures.Queries;
using Mariage.Contracts.Gift;
using Mariage.Contracts.Pictures;
using Mariage.Domain.GiftAggregate.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mariage.Api.Controllers;

public static class WeddingListController
{
    public static IApplicationBuilder UseWeddingListController(this IApplicationBuilder builder)
    {
        return builder.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("/wedding-list",
                    async (IMediator mediator, IMapper mapper) =>
                    {
                        var query = new GetGiftQuery();
                        var getGiftResult = await mediator.Send(query);
                        
                        return getGiftResult.Match(
                            createGiftResult =>
                            {
                                var gift = mapper.Map<List<GiftResponse>>(createGiftResult);
                                return Results.Ok(gift);
                            },
                            error => error.Result()
                        );
                    })
                .WithName("GetWeddingList")
                .WithOpenApi();
            
            endpoints.MapGet("/wedding-list/gift/{giftId}",
                    async (IMediator mediator, IMapper mapper, Guid giftId) =>
                    {
                        var query = new GetGiftByIdQuery(GiftId.Create(giftId));
                        var getGiftByIdResult = await mediator.Send(query);
                        
                        return getGiftByIdResult.Match(
                            getGiftByIdResult =>
                            {
                                var gift = mapper.Map<GiftResponse>(getGiftByIdResult);
                                return Results.Ok(gift);
                            },
                            error => error.Result()
                        );
                    })
                .WithName("GetGiftById")
                .WithOpenApi();

            endpoints.MapPost("/wedding-list",
                    async (
                        IMediator mediator,
                        IMapper mapper,
                        IBlobService blobService,
                        [FromForm] CreateGiftRequest request) =>
                    {
                        if (request.ImageFile.Length == 0)
                        {
                            return Results.BadRequest("Image file is required.");
                        }
                        
                        var fileName = Path.GetFileName(request.ImageFile.FileName);
                        await using var stream = request.ImageFile.OpenReadStream();
                        var imageUrl = await blobService.UploadFileAsync(stream, fileName);
                        
                        Console.WriteLine(request.Category + 1);
                        var command = mapper.Map<CreateGiftCommand>((request, imageUrl));
                        Console.WriteLine(command.Category.Value);
                        var createGiftResult = await mediator.Send(command);
                        
                        return createGiftResult.Match(
                            createGiftResult =>
                            {
                                var gift = mapper.Map<GiftResponse>(createGiftResult);
                                return Results.Ok(gift);
                            },
                            error => error.Result()
                        );
                    })
                .RequireAuthorization("IsAdmin")
                .WithName("CreateGift")
                .DisableAntiforgery()
                .WithOpenApi();
            
            endpoints.MapPost("/wedding-list/{giftId}/participation",
                    async (IMediator mediator, IMapper mapper, CreateGiftParticipationRequest request, Guid giftId) =>
                    {
                        var command = mapper.Map<CreateGiftParticipationCommand>((request, giftId));
                        var createGiftParticipationResult = await mediator.Send(command);
                        
                        return createGiftParticipationResult.Match(
                            createGiftParticipationResult =>
                            {
                                var gift = mapper.Map<GiftResponse>(createGiftParticipationResult);
                                return Results.Ok(gift);
                            },
                            error => error.Result()
                        );
                    })
                .WithName("CreateGiftParticipation")
                .WithOpenApi();
        });
    }
}