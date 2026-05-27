using ErrorOr;
using Mariage.Domain.AccommodationAggregate;
using Mariage.Domain.UserAggregate.ValueObjects;
using MediatR;

namespace Mariage.Application.Accommodations.Queries.GetMyBookings;

public record GetMyBookingsQuery(UserId UserId) : IRequest<ErrorOr<List<Accommodation>>>;
