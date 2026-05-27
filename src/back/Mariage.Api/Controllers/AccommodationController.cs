using System.Security.Claims;
using MapsterMapper;
using Mariage.Api.Errors;
using Mariage.Application.Accommodations.Commands.BookAccommodation;
using Mariage.Application.Accommodations.Commands.CancelBooking;
using Mariage.Application.Accommodations.Commands.ConfirmBooking;
using Mariage.Application.Accommodations.Commands.CreateAccommodation;
using Mariage.Application.Accommodations.Commands.DeleteAccommodation;
using Mariage.Application.Accommodations.Commands.UpdateAccommodation;
using Mariage.Application.Accommodations.Queries.GetAccommodations;
using Mariage.Application.Accommodations.Queries.GetAvailableAccommodations;
using Mariage.Application.Accommodations.Queries.GetMyBookings;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Application.Common.Interfaces.Services;
using Mariage.Contracts.Accommodation;
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
            // === Admin endpoints ===

            endpoints.MapGet("/accommodations",
                    async (IMediator mediator, IUserRepository userRepository) =>
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
                                    a.PricePerPersonPerNight,
                                    a.NumberOfNights,
                                    a.Capacity,
                                    a.RemainingCapacity,
                                    a.Bookings.Select(b => new BookingResponse(
                                        b.Id.Value,
                                        a.Id.Value,
                                        a.Title,
                                        a.UrlImage,
                                        b.NumberOfPersons,
                                        b.TotalAmount,
                                        b.BookingStatus.ToString(),
                                        b.BookerFirstName,
                                        b.BookerLastName,
                                        b.BookerEmail,
                                        b.CreatedAt
                                    )).ToList()
                                )).ToList();
                                return Results.Ok(response);
                            },
                            error => error.Result());
                    })
                .RequireAuthorization("IsAdmin")
                .WithName("GetAccommodations");

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

                        var command = new CreateAccommodationCommand(
                            request.Title,
                            request.Description,
                            imageUrl,
                            request.PricePerPersonPerNight,
                            request.NumberOfNights,
                            request.Capacity);
                        var result = await mediator.Send(command);

                        return result.Match(
                            accommodation => Results.Ok(MapToResponse(accommodation)),
                            error => error.Result());
                    })
                .RequireAuthorization("IsAdmin")
                .WithName("CreateAccommodation")
                .DisableAntiforgery();

            endpoints.MapPut("/accommodations/{id}",
                    async (
                        IMediator mediator,
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

                        var command = new UpdateAccommodationCommand(
                            AccommodationId.Create(id),
                            request.Title,
                            request.Description,
                            finalImageUrl,
                            request.PricePerPersonPerNight,
                            request.NumberOfNights,
                            request.Capacity);
                        var result = await mediator.Send(command);

                        return result.Match(
                            accommodation => Results.Ok(MapToResponse(accommodation)),
                            error => error.Result());
                    })
                .RequireAuthorization("IsAdmin")
                .WithName("UpdateAccommodation")
                .DisableAntiforgery();

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
                .WithName("DeleteAccommodation");

            // === Guest endpoints ===

            endpoints.MapGet("/accommodations/available",
                    async (IMediator mediator) =>
                    {
                        var query = new GetAvailableAccommodationsQuery();
                        var result = await mediator.Send(query);

                        return result.Match(
                            accommodations =>
                            {
                                var response = accommodations.Select(a => new AvailableAccommodationResponse(
                                    a.Id.Value,
                                    a.Title,
                                    a.Description,
                                    a.UrlImage,
                                    a.PricePerPersonPerNight,
                                    a.NumberOfNights,
                                    a.Capacity,
                                    a.RemainingCapacity
                                )).ToList();
                                return Results.Ok(response);
                            },
                            error => error.Result());
                    })
                .RequireAuthorization()
                .WithName("GetAvailableAccommodations");

            endpoints.MapPost("/accommodations/{id}/bookings",
                    async (IMediator mediator, HttpContext httpContext, BookAccommodationRequest request, Guid id) =>
                    {
                        var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                        if (userIdClaim == null)
                            return Results.BadRequest("User ID claim not found in token.");

                        var command = new BookAccommodationCommand(
                            AccommodationId.Create(id),
                            UserId.Create(Guid.Parse(userIdClaim)),
                            request.NumberOfPersons,
                            request.FirstName,
                            request.LastName,
                            request.Email);
                        var result = await mediator.Send(command);

                        return result.Match(
                            accommodation => Results.Ok(MapToResponse(accommodation)),
                            error => error.Result());
                    })
                .RequireAuthorization()
                .WithName("BookAccommodation");

            endpoints.MapPut("/accommodations/{id}/bookings/{bookingId}/confirm",
                    async (IMediator mediator, HttpContext httpContext, Guid id, Guid bookingId) =>
                    {
                        var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                        if (userIdClaim == null)
                            return Results.BadRequest("User ID claim not found in token.");

                        var command = new ConfirmBookingCommand(
                            AccommodationId.Create(id),
                            AccommodationBookingId.Create(bookingId),
                            UserId.Create(Guid.Parse(userIdClaim)));
                        var result = await mediator.Send(command);

                        return result.Match(
                            accommodation => Results.Ok(MapToResponse(accommodation)),
                            error => error.Result());
                    })
                .RequireAuthorization()
                .WithName("ConfirmBooking");

            endpoints.MapDelete("/accommodations/{id}/bookings/{bookingId}",
                    async (IMediator mediator, HttpContext httpContext, Guid id, Guid bookingId) =>
                    {
                        var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                        if (userIdClaim == null)
                            return Results.BadRequest("User ID claim not found in token.");

                        var command = new CancelBookingCommand(
                            AccommodationId.Create(id),
                            AccommodationBookingId.Create(bookingId),
                            UserId.Create(Guid.Parse(userIdClaim)));
                        var result = await mediator.Send(command);

                        return result.Match(
                            _ => Results.NoContent(),
                            error => error.Result());
                    })
                .RequireAuthorization()
                .WithName("CancelBooking");

            endpoints.MapGet("/accommodations/my-bookings",
                    async (IMediator mediator, HttpContext httpContext) =>
                    {
                        var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                        if (userIdClaim == null)
                            return Results.BadRequest("User ID claim not found in token.");

                        var userId = UserId.Create(Guid.Parse(userIdClaim));
                        var query = new GetMyBookingsQuery(userId);
                        var result = await mediator.Send(query);

                        return result.Match(
                            accommodations =>
                            {
                                var bookings = accommodations
                                    .SelectMany(a => a.Bookings
                                        .Where(b => b.UserId == userId)
                                        .Select(b => new BookingResponse(
                                            b.Id.Value,
                                            a.Id.Value,
                                            a.Title,
                                            a.UrlImage,
                                            b.NumberOfPersons,
                                            b.TotalAmount,
                                            b.BookingStatus.ToString(),
                                            b.BookerFirstName,
                                            b.BookerLastName,
                                            b.BookerEmail,
                                            b.CreatedAt)))
                                    .ToList();
                                return Results.Ok(new MyBookingsResponse(bookings));
                            },
                            error => error.Result());
                    })
                .RequireAuthorization()
                .WithName("GetMyBookings");
        });
    }

    private static AccommodationResponse MapToResponse(Domain.AccommodationAggregate.Accommodation accommodation)
    {
        return new AccommodationResponse(
            accommodation.Id.Value,
            accommodation.Title,
            accommodation.Description,
            accommodation.UrlImage,
            accommodation.PricePerPersonPerNight,
            accommodation.NumberOfNights,
            accommodation.Capacity,
            accommodation.RemainingCapacity,
            accommodation.Bookings.Select(b => new BookingResponse(
                b.Id.Value,
                accommodation.Id.Value,
                accommodation.Title,
                accommodation.UrlImage,
                b.NumberOfPersons,
                b.TotalAmount,
                b.BookingStatus.ToString(),
                b.BookerFirstName,
                b.BookerLastName,
                b.BookerEmail,
                b.CreatedAt
            )).ToList());
    }
}
