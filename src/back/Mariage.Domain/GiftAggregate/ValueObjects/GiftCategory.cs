using Mariage.Domain.Common.Models;

namespace Mariage.Domain.GiftAggregate.ValueObjects;
public sealed class GiftCategory : ValueObject
{
    public enum CategoryType
    {
        HomeAppliances,
        Decorations,
        TableArts,
        Digestives,
        Furniture,
        HouseholdLinens,
        Kitchenware,
        Santons,
        Honeymoon,
    }

    public GiftCategory()
    {
    }

    public GiftCategory(int category)
    {
        Value = (CategoryType)category;
    }
    
    public CategoryType Value { get; protected set; }

    public static GiftCategory Create(CategoryType category)
    {
        return new GiftCategory((int)category);
    }
    
    public static GiftCategory Create(int category)
    {
        return new GiftCategory(category);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
