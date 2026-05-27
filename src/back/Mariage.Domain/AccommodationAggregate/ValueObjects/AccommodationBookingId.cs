using Mariage.Domain.Common.Models;

namespace Mariage.Domain.AccommodationAggregate.ValueObjects;

public sealed class AccommodationBookingId(Guid value) : ValueObject
{
    public Guid Value { get; protected set; } = value;

    public static AccommodationBookingId CreateUnique()
    {
        return new AccommodationBookingId(Guid.NewGuid());
    }

    public static AccommodationBookingId Create(Guid value)
    {
        return new AccommodationBookingId(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
