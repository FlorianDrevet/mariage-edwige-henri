using ErrorOr;
using MapsterMapper;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Application.Common.Models;
using Mariage.Application.Pictures.Common;
using Mariage.Domain.Common.Errors;
using MediatR;

namespace Mariage.Application.Pictures.Queries.GetFavoritesPictures;

public class GetFavoritePicturesHandler(IPictureRepository pictureRepository, IUserRepository userRepository, IMapper mapper)
    : IRequestHandler<GetFavoritePicturesQuery, ErrorOr<PaginatedList<PictureResult>>>
{
    public async Task<ErrorOr<PaginatedList<PictureResult>>> Handle(GetFavoritePicturesQuery command, CancellationToken cancellationToken)
    {
        var user = userRepository.GetUserById(command.UserId);
        if (user is null)
        {
            return Errors.User.NotFoundUserWithIdError();
        }

        var allFavoritePictureIds = user.PictureIds;
        var totalCount = allFavoritePictureIds.Count;
        var pagedPictureIds = allFavoritePictureIds
            .Skip((command.PageNumber - 1) * command.PageSize)
            .Take(command.PageSize);

        List<PictureResult> pictureResults = new();
        foreach (var pictureId in pagedPictureIds)
        {
            var picture = pictureRepository.GetPictureById(pictureId);
            if (picture is null)
            {
                return Errors.Pictures.NotFoundPictureWithIdError();
            }
            pictureResults.Add(mapper.Map<PictureResult>((picture, user)));
        }

        return new PaginatedList<PictureResult>(
            pictureResults,
            totalCount,
            command.PageNumber,
            command.PageSize);
    }
}