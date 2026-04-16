using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Application.Common.Models;
using Mariage.Domain.PictureAggregate;
using Mariage.Domain.PictureAggregate.ValueObject;
using Mariage.Domain.UserAggregate.ValueObjects;
using Mariage.Infrastructure.Extensions;
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

    public async Task<PaginatedList<Picture>> GetPicturesAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var query = mariageDbContext.Pictures.OrderByDescending(p => p.CreatedAt);
        return await query.ToPaginatedListAsync(pageNumber, pageSize, cancellationToken);
    }

    public Picture? GetPictureById(PictureId pictureId)
    {
        return mariageDbContext.Pictures.FirstOrDefault(p => p.Id == pictureId);
    }

    public async Task<PaginatedList<Picture>> GetPicturesTookByUserAsync(int pageNumber, int pageSize, UserId userId, CancellationToken cancellationToken)
    {
        var query = mariageDbContext.Pictures
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.CreatedAt);
        return await query.ToPaginatedListAsync(pageNumber, pageSize, cancellationToken);
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