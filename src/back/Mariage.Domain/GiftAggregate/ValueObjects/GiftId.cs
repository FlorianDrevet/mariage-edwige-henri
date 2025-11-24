using Mariage.Domain.Common.Models;

namespace Mariage.Domain.GiftAggregate.ValueObjects;

public sealed class GiftId(Guid value) : ValueObject
{
    public Guid Value { get; protected set; } = value;

    public static GiftId CreateUnique()
    {
        return new GiftId(Guid.NewGuid());
    }

    public static GiftId Create(Guid value)
    {
        return new GiftId(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}