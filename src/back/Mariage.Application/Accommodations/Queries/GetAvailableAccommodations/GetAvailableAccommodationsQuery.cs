using ErrorOr;
using Mariage.Domain.AccommodationAggregate;
using MediatR;

namespace Mariage.Application.Accommodations.Queries.GetAvailableAccommodations;

public record GetAvailableAccommodationsQuery() : IRequest<ErrorOr<List<Accommodation>>>;
