using Mariage.Domain.Common.Models;

namespace Mariage.Domain.UserAggregate.ValueObjects;

public sealed class GuestId(Guid value) : ValueObject
{
    public Guid Value { get; protected set; } = value;

    public static GuestId CreateUnique()
    {
        return new GuestId(Guid.NewGuid());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static GuestId Create(Guid value)
    {
        return new GuestId(value);
    }
}
