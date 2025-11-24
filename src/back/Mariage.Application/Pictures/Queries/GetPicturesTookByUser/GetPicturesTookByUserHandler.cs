using ErrorOr;
using MapsterMapper;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Application.Pictures.Common;
using Mariage.Domain.Common.Errors;
using Mariage.Domain.PictureAggregate;
using MediatR;

namespace Mariage.Application.Pictures.Queries.GetPicturesTookByUser;

public class GetPicturesTookByUserHandler(IUserRepository userRepository, IPictureRepository pictureRepository, IMapper mapper)
    : IRequestHandler<GetPicturesTookByUserQuery, ErrorOr<List<PictureResult>>>
{
    public async Task<ErrorOr<List<PictureResult>>> Handle(GetPicturesTookByUserQuery command, CancellationToken cancellationToken)
    {
        var user = userRepository.GetUserById(command.UserId);
        if (user is null)
        {
            return Errors.User.NotFoundUserWithIdError();
        }

        List<Picture> pictures = pictureRepository.GetPicturesTookByUser(command.Page, command.PageSize, command.UserId);
        return  pictures.Select(picture => mapper.Map<PictureResult>((picture, user))).ToList();
    }
}