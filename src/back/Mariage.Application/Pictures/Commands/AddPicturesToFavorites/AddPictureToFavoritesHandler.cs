using ErrorOr;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Domain.Common.Errors;
using MediatR;

namespace Mariage.Application.Pictures.Commands.AddPicturesToFavorites;

public class AddPictureToFavoritesHandler(IUserRepository userRepository, IPictureRepository pictureRepository)
    : IRequestHandler<AddPicturesToFavoritesCommand, ErrorOr<bool>>
{
    public async Task<ErrorOr<bool>> Handle(AddPicturesToFavoritesCommand command, CancellationToken cancellationToken)
    {
        
        var user = userRepository.GetUserById(command.UserId);
        
        if (user == null)
        {
            return Errors.User.NotFoundUserWithIdError();
        }
        
        user.AddPictureToFavorite(command.PictureId);
        userRepository.UpdateUser(user);
        
        return true;
    }
}