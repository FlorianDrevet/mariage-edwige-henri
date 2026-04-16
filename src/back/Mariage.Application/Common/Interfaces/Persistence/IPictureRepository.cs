using Mariage.Application.Common.Models;
using Mariage.Domain.PictureAggregate;
using Mariage.Domain.PictureAggregate.ValueObject;
using Mariage.Domain.UserAggregate.ValueObjects;

namespace Mariage.Application.Common.Interfaces.Persistence;

public interface IPictureRepository
{
   List<Picture> GetPictureByUserId(UserId userId);
   Picture AddPicture(Picture picture);
   Task<PaginatedList<Picture>> GetPicturesAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
   Picture? GetPictureById(PictureId pictureId);
   Task<PaginatedList<Picture>> GetPicturesTookByUserAsync(int pageNumber, int pageSize, UserId userId, CancellationToken cancellationToken = default);
   bool RemovePicture(PictureId pictureId);
}