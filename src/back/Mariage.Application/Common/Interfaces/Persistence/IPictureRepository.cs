using ErrorOr;
using Mariage.Application.Pictures.Common;
using Mariage.Domain.PictureAggregate;
using Mariage.Domain.PictureAggregate.ValueObject;
using Mariage.Domain.UserAggregate.ValueObjects;

namespace Mariage.Application.Common.Interfaces.Persistence;

public interface IPictureRepository
{
   List<Picture> GetPictureByUserId(UserId userId);
   Picture AddPicture(Picture picture);
   List<Picture> GetPictures(int page, int pageSize);
   Picture? GetPictureById(PictureId pictureId);
   List<Picture> GetPicturesTookByUser(int page, int pageSize, UserId userId);
   bool RemovePicture(PictureId pictureId);
}