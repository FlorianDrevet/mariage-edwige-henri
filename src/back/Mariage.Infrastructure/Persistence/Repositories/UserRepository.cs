using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Domain.AccommodationAggregate.ValueObjects;
using Mariage.Domain.PictureAggregate.ValueObject;
using Mariage.Domain.UserAggregate;
using Mariage.Domain.UserAggregate.Entities;
using Mariage.Domain.UserAggregate.ValueObjects;

namespace Mariage.Infrastructure.Persistence.Repositories;

public class UserRepository(MariageDbContext mariageDbContext) : IUserRepository
{
    public User? GetUserByUsername(string username)
    {
        return mariageDbContext.Users
            .FirstOrDefault(user => user.Username == username);
    }

    public void AddUser(User user)
    {
        mariageDbContext.Add(user);
        mariageDbContext.SaveChanges();
    }

    public User? GetUserById(UserId requestUserId)
    {
        return mariageDbContext.Users
            .FirstOrDefault(user => user.Id == requestUserId);
    }

    public void UpdateUser(User user)
    {
        mariageDbContext.Update(user);
        mariageDbContext.SaveChanges();
    }

    public List<User> GetAllUsers()
    {
        return mariageDbContext.Users.ToList();
    }

    public void DeletePicture(PictureId pictureId)
    {
        var users = mariageDbContext.Users.ToList();
        users = users.Where(user => user.PictureIds.Contains(pictureId)).ToList();
        foreach (var user in users)
        {
            user.PictureIds.Remove(pictureId);
            mariageDbContext.Update(user);
        }
    }


    public User AddGuests(UserId requestUserId, List<Guest> guests)
    {
        var user = mariageDbContext.Users
            .FirstOrDefault(user => user.Id == requestUserId);
        user!.AddGuests(guests);
        mariageDbContext.SaveChanges();
        return user;
    }

    public List<User> GetUsersByAccommodationId(AccommodationId accommodationId)
    {
        return mariageDbContext.Users
            .Where(user => user.AccommodationId == accommodationId)
            .ToList();
    }

    public void UpdateUsers(IEnumerable<User> users)
    {
        foreach (var user in users)
        {
            mariageDbContext.Update(user);
        }
        mariageDbContext.SaveChanges();
    }
}