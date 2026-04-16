using ErrorOr;
using Mariage.Domain.AccommodationAggregate;
using MediatR;

namespace Mariage.Application.Accommodations.Queries.GetAllAccommodations;

public record GetAllAccommodationsQuery() : IRequest<ErrorOr<List<Accommodation>>>;
