using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Application.Pictures.Common;
using Mariage.Domain.PictureAggregate;
using Mariage.Domain.PictureAggregate.ValueObject;
using Mariage.Domain.UserAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Mariage.Infrastructure.Persistence.Repositories;

public class PictureRepository(MariageDbContext mariageDbContext): IPictureRepository
{
    public List<Picture> GetPictureByUserId(UserId userId)
    {
        return mariageDbContext.Pictures.Where(p => p.UserId == userId).ToList();
    }

    public Picture AddPicture(Picture picture)
    {
        mariageDbContext.Add(picture);
        mariageDbContext.SaveChanges();
        return picture;
    }

    public List<Picture> GetPictures(int page, int pageSize)
    {
        return mariageDbContext.Pictures.OrderByDescending(p => p.CreatedAt)
            .Skip(page * pageSize).Take(pageSize).ToList();
    }

    public Picture? GetPictureById(PictureId pictureId)
    {
        return mariageDbContext.Pictures.FirstOrDefault(p => p.Id == pictureId);
    }

    public List<Picture> GetPicturesTookByUser(int page, int pageSize, UserId userId)
    {
        return mariageDbContext.Pictures.Where(p => p.UserId == userId).Skip(page * pageSize).Take(pageSize).ToList();
    }

    public bool RemovePicture(PictureId pictureId)
    {
        var picture = GetPictureById(pictureId);
        if (picture == null)
        {
            return false;
        }

        mariageDbContext.Pictures.Remove(picture);
        mariageDbContext.SaveChanges();
        return true;
    }
}