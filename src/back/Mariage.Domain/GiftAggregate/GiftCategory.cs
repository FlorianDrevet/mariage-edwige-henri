using Mariage.Domain.Common.Models;
using Mariage.Domain.GiftAggregate.ValueObjects;

namespace Mariage.Domain.GiftAggregate;

public sealed class GiftCategory : AggregateRoot<GiftCategoryId>
{
    public string Name { get; private set; } = null!;

    private GiftCategory(GiftCategoryId id, string name) : base(id)
    {
        Name = name;
    }

    public static GiftCategory Create(string name)
    {
        return new GiftCategory(GiftCategoryId.CreateUnique(), name);
    }

    public GiftCategory() { }
}
