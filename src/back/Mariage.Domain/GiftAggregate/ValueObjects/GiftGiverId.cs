using Mariage.Domain.Common.Models;

namespace Mariage.Domain.GiftAggregate.ValueObjects;

public sealed class GiftGiverId(Guid value) : ValueObject
{
    public Guid Value { get; protected set; } = value;
    
    public static GiftGiverId CreateUnique()
    {
        return new GiftGiverId(Guid.NewGuid());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static GiftGiverId Create(Guid guid)
    {
        return new GiftGiverId(guid);
    }
}