using ErrorOr;
using Mariage.Domain.Common.Errors;
using Mariage.Domain.Common.Models;
using Mariage.Domain.PictureAggregate;
using Mariage.Domain.PictureAggregate.ValueObject;
using Mariage.Domain.UserAggregate.Entities;
using Mariage.Domain.UserAggregate.ValueObjects;

namespace Mariage.Domain.UserAggregate;

public sealed class User : AggregateRoot<UserId>
{
    private readonly List<Guest> _guests = new();
    public string Username { get; private set; } = null!;
    public string? Email { get; private set; } = null!;
    public string Password { get; private set; } = null!;
    public string Salt { get; private set; } = null!;
    public string Role { get; set; } = null!;
    
    public List<PictureId> PictureIds { get; private set; } = new();
    
    public IReadOnlyList<Guest> Guests => _guests.AsReadOnly();

    private User(UserId userId, string username, string password, string salt, List<PictureId> pictureIds)
        : base(userId)
    {
        Username = username;
        Password = password;
        Salt = salt;
        Role = "User";
    }
    
    public static User Create(string username, string password, string salt)
    {
        return new User(UserId.CreateUnique(), username, password, salt, new List<PictureId>());
    }
    
    public User(){}

    public void ChangeEmail(string requestEmail)
    {
        Email = requestEmail;
    }

    public void ChangeIsComing(GuestId guestId, bool isComing)
    {
        var guest = _guests.FirstOrDefault(x => x.Id == guestId);
        if (guest is not null)
        {
            guest.ChangeIsComing(isComing);
        }
    }

    public void AddGuests(List<Guest> guests)
    {
        _guests.AddRange(guests);
    }

    public bool UpdateGuest(GuestId guestId, string firstName, string lastName)
    {
        var guest = _guests.FirstOrDefault(x => x.Id == guestId);
        if (guest is null) return false;
        guest.Update(firstName, lastName);
        return true;
    }

    public bool RemoveGuest(GuestId guestId)
    {
        var guest = _guests.FirstOrDefault(x => x.Id == guestId);
        if (guest is null) return false;
        _guests.Remove(guest);
        return true;
    }
    
    public bool AddPictureToFavorite(PictureId pictureId)
    {
        // check if exists
        if (PictureIds.Contains(pictureId))
        {
            return false;
        }
        PictureIds.Add(pictureId);
        return true;
    }
    
    public bool RemovePictureFromFavorite(PictureId pictureId)
    {
        return PictureIds.Remove(pictureId);
    }
}