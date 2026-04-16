using Mariage.Domain.Common.Models;

namespace Mariage.Domain.AccommodationAggregate.ValueObjects;

public sealed class AccommodationId(Guid value) : ValueObject
{
    public Guid Value { get; protected set; } = value;

    public static AccommodationId CreateUnique()
    {
        return new AccommodationId(Guid.NewGuid());
    }

    public static AccommodationId Create(Guid value)
    {
        return new AccommodationId(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
