using Mariage.Domain.AccommodationAggregate.Enums;
using Mariage.Domain.AccommodationAggregate.ValueObjects;
using Mariage.Domain.Common.Models;
using Mariage.Domain.UserAggregate.ValueObjects;

namespace Mariage.Domain.AccommodationAggregate.Entities;

public sealed class AccommodationAssignment : Entity<AccommodationAssignmentId>
{
    public UserId UserId { get; private set; } = null!;
    public AccommodationResponseStatus ResponseStatus { get; private set; }

    private AccommodationAssignment(
        AccommodationAssignmentId id,
        UserId userId,
        AccommodationResponseStatus responseStatus) : base(id)
    {
        UserId = userId;
        ResponseStatus = responseStatus;
    }

    public static AccommodationAssignment Create(UserId userId)
    {
        return new AccommodationAssignment(
            AccommodationAssignmentId.CreateUnique(),
            userId,
            AccommodationResponseStatus.Pending);
    }

    public void Respond(AccommodationResponseStatus status)
    {
        ResponseStatus = status;
    }

    public AccommodationAssignment() { }
}
