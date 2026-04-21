using Mariage.Domain.Common.Models;

namespace Mariage.Domain.GiftAggregate.ValueObjects;

public sealed class GiftCategoryId(Guid value) : ValueObject
{
    public Guid Value { get; protected set; } = value;

    public static GiftCategoryId CreateUnique()
    {
        return new GiftCategoryId(Guid.NewGuid());
    }

    public static GiftCategoryId Create(Guid value)
    {
        return new GiftCategoryId(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
