using Mariage.Domain.Common.Models;
using Mariage.Domain.GiftAggregate.Entities;
using Mariage.Domain.GiftAggregate.ValueObjects;

namespace Mariage.Domain.GiftAggregate;

public sealed class Gift : AggregateRoot<GiftId>
{
    // ReSharper disable once CollectionNeverUpdated.Local
    private readonly List<GiftGiver> _giftGivers = new();
    
    public string Name { get; private set; } = null!;
    public float Price { get; private set; }
    public float Participation { get; private set; } = 0;
    public string UrlImage { get; private set; } = null!;
    public GiftCategory Category { get; private set; } = null!;
    
    public IReadOnlyList<GiftGiver> GiftGivers => _giftGivers.AsReadOnly();
    
    private Gift(GiftId giftId, string name, float price, string urlImage, GiftCategory giftCategory) : base(giftId)
    {
        Name = name;
        Price = price;
        UrlImage = urlImage;
        Category = giftCategory;
    }
    
    public static Gift Create(string name, float price, string urlImage, GiftCategory giftCategory)
    {
        return new Gift(GiftId.CreateUnique(), name, price, urlImage, giftCategory);
    }
    
    public Gift(){}

    public void AddGiftGiver(GiftGiver giftGiver)
    {
        _giftGivers.Add(giftGiver);
        Participation += giftGiver.Amount;
    }

    public void Update(string name, float price, string urlImage, GiftCategory category)
    {
        Name = name;
        Price = price;
        UrlImage = urlImage;
        Category = category;
    }
} 