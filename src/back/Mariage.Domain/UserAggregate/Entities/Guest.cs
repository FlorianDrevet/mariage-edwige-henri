using Mariage.Domain.Common.Models;
using Mariage.Domain.GiftAggregate.Entities;
using Mariage.Domain.UserAggregate.ValueObjects;

namespace Mariage.Domain.UserAggregate.Entities;

public sealed class Guest: Entity<GuestId>
{
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public bool IsComing { get; private set; } = false;
    
    private Guest(GuestId guestId, string firstName, string lastName) :
        base(guestId)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public static Guest Create(string firstName, string lastName)
    {
        return new Guest(GuestId.CreateUnique(), firstName, lastName);
    }
    
    public Guest(){}

    public void ChangeIsComing(bool isComing)
    {
        IsComing = isComing;
    }
}