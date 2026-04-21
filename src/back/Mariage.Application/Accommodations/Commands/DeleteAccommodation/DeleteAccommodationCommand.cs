using ErrorOr;
using Mariage.Domain.AccommodationAggregate.ValueObjects;
using MediatR;

namespace Mariage.Application.Accommodations.Commands.DeleteAccommodation;

public record DeleteAccommodationCommand(
    AccommodationId AccommodationId
) : IRequest<ErrorOr<Deleted>>;
