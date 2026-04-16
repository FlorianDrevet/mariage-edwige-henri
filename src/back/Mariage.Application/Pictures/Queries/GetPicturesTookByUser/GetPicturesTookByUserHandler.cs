using ErrorOr;
using MapsterMapper;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Application.Common.Models;
using Mariage.Application.Pictures.Common;
using Mariage.Domain.Common.Errors;
using MediatR;

namespace Mariage.Application.Pictures.Queries.GetPicturesTookByUser;

public class GetPicturesTookByUserHandler(IUserRepository userRepository, IPictureRepository pictureRepository, IMapper mapper)
    : IRequestHandler<GetPicturesTookByUserQuery, ErrorOr<PaginatedList<PictureResult>>>
{
    public async Task<ErrorOr<PaginatedList<PictureResult>>> Handle(GetPicturesTookByUserQuery command, CancellationToken cancellationToken)
    {
        var user = userRepository.GetUserById(command.UserId);
        if (user is null)
        {
            return Errors.User.NotFoundUserWithIdError();
        }

        var paginatedPictures = await pictureRepository.GetPicturesTookByUserAsync(
            command.PageNumber, command.PageSize, command.UserId, cancellationToken);

        var pictureResults = paginatedPictures.Items
            .Select(picture => mapper.Map<PictureResult>((picture, user)))
            .ToList();

        return new PaginatedList<PictureResult>(
            pictureResults,
            paginatedPictures.TotalCount,
            paginatedPictures.PageNumber,
            paginatedPictures.PageSize);
    }
}