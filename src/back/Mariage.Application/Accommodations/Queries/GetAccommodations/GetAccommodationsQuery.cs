using ErrorOr;
using Mariage.Domain.AccommodationAggregate;
using MediatR;

namespace Mariage.Application.Accommodations.Queries.GetAccommodations;

public record GetAccommodationsQuery() : IRequest<ErrorOr<List<Accommodation>>>;
