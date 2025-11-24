using ErrorOr;
using MapsterMapper;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Application.Pictures.Common;
using Mariage.Domain.Common.Errors;
using MediatR;

namespace Mariage.Application.Pictures.Queries.GetFavoritesPictures;

public class GetFavoritePicturesHandler(IPictureRepository pictureRepository, IUserRepository userRepository, IMapper mapper)
    : IRequestHandler<GetFavoritePicturesQuery, ErrorOr<List<PictureResult>>>
{
    public async Task<ErrorOr<List<PictureResult>>> Handle(GetFavoritePicturesQuery command, CancellationToken cancellationToken)
    {
        var user = userRepository.GetUserById(command.UserId);
        if (user is null)
        {
            return Errors.User.NotFoundUserWithIdError();
        }
        
        List<PictureResult> pictureResults = new();
        foreach (var pictureId in user.PictureIds.Skip(command.Page * command.PageSize).Take(command.PageSize))
        {
            var picture = pictureRepository.GetPictureById(pictureId);
            if (picture is null)
            {
                return Errors.Pictures.NotFoundPictureWithIdError();
            }
            pictureResults.Add(mapper.Map<PictureResult>((picture, user)));
        }

        return pictureResults;
    }
}