using Mariage.Domain.Common.Models;
using Mariage.Domain.GiftAggregate.ValueObjects;

namespace Mariage.Domain.GiftAggregate.Entities;

public sealed class GiftGiver: Entity<GiftGiverId>
{
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public string? Email { get; private set; } = null!;
    public float Amount { get; private set; }

    private GiftGiver(GiftGiverId giftGiverId, string firstName, string lastName, string email, float amount) :
        base(giftGiverId)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Amount = amount;
    }

    public static GiftGiver Create(string firstName, string lastName, string email, float amount)
    {
        return new GiftGiver(GiftGiverId.CreateUnique(), firstName, lastName, email, amount);
    }
    
    public GiftGiver(){}
}