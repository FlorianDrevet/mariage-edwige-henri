using Mariage.Domain.UserAggregate.ValueObjects;

namespace Mariage.Domain.PictureAggregate.ValueObject;

public sealed class PictureId(Guid value) : Common.Models.ValueObject
{
    public Guid Value { get; protected set; } = value;

    public static PictureId CreateUnique()
    {
        return new PictureId(Guid.NewGuid());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static PictureId Create(Guid value)
    {
        return new PictureId(value);
    }
}