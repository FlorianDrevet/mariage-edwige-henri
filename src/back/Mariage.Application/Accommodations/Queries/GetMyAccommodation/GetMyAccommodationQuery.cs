using ErrorOr;
using Mariage.Domain.AccommodationAggregate;
using Mariage.Domain.UserAggregate.ValueObjects;
using MediatR;

namespace Mariage.Application.Accommodations.Queries.GetMyAccommodation;

public record GetMyAccommodationQuery(
    UserId UserId
) : IRequest<ErrorOr<Accommodation>>;
