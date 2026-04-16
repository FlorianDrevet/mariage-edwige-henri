using ErrorOr;
using Mariage.Domain.PictureAggregate.ValueObject;
using Mariage.Domain.UserAggregate;
using Mariage.Domain.UserAggregate.Entities;
using Mariage.Domain.UserAggregate.ValueObjects;

namespace Mariage.Application.Common.Interfaces.Persistence;

public interface IUserRepository
{
    User? GetUserByUsername(string username);
    void AddUser(User user);
    User? GetUserById(UserId requestUserId);
    void UpdateUser(User user);
    void DeleteUser(User user);
    List<User> GetAllUsers();
    void DeletePicture(PictureId pictureId);
    User AddGuests(UserId requestUserId, List<Guest> guests);
}