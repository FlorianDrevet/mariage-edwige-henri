using ErrorOr;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Domain.Common.Errors;
using MediatR;

namespace Mariage.Application.Pictures.Commands.RemovePictureFromFavorites;

public class RemovePictureFromFavoritesHandler(IUserRepository userRepository, IPictureRepository pictureRepository)
    : IRequestHandler<RemovePictureFromFavoritesCommand, ErrorOr<bool>>
{
    public async Task<ErrorOr<bool>> Handle(RemovePictureFromFavoritesCommand command, CancellationToken cancellationToken)
    {
        var user = userRepository.GetUserById(command.UserId);
        
        if (user == null)
        {
            return Errors.User.NotFoundUserWithIdError();
        }
        
        user.RemovePictureFromFavorite(command.PictureId);
        userRepository.UpdateUser(user);
        
        return true;
    }
}