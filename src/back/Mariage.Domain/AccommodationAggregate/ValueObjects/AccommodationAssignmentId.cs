using Mariage.Domain.Common.Models;

namespace Mariage.Domain.AccommodationAggregate.ValueObjects;

public sealed class AccommodationAssignmentId(Guid value) : ValueObject
{
    public Guid Value { get; protected set; } = value;

    public static AccommodationAssignmentId CreateUnique()
    {
        return new AccommodationAssignmentId(Guid.NewGuid());
    }

    public static AccommodationAssignmentId Create(Guid value)
    {
        return new AccommodationAssignmentId(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
