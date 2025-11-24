using ErrorOr;
using Mariage.Application.Common.Interfaces.Persistence;
using MediatR;

namespace Mariage.Application.Pictures.Commands.RemovePicture;

public class RemovePictureHandler(IPictureRepository pictureRepository, IUserRepository userRepository)
    : IRequestHandler<RemovePictureCommand, ErrorOr<bool>>
{
    public async Task<ErrorOr<bool>> Handle(RemovePictureCommand command, CancellationToken cancellationToken)
    {
        var user = userRepository.GetUserById(command.UserId);
        var picture = pictureRepository.GetPictureById(command.PictureId);
        if (user is null || (user.Role != "Admin" && (picture is null || user.Id != picture.UserId)))
            return false;
        userRepository.DeletePicture(command.PictureId);
        return pictureRepository.RemovePicture(command.PictureId);
    }
}