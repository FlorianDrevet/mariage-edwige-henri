using ErrorOr;
using MapsterMapper;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Application.Common.Models;
using Mariage.Application.Pictures.Common;
using MediatR;

namespace Mariage.Application.Pictures.Queries;

public class GetPictureQueryHandler(IPictureRepository pictureRepository, IUserRepository userRepository, IMapper mapper)
    : IRequestHandler<GetPictureQuery, ErrorOr<PaginatedList<PictureResult>>>
{
    public async Task<ErrorOr<PaginatedList<PictureResult>>> Handle(GetPictureQuery request, CancellationToken cancellationToken)
    {
        var paginatedPictures = await pictureRepository.GetPicturesAsync(request.PageNumber, request.PageSize, cancellationToken);
        
        List<PictureResult> pictureResults = new();
        foreach (var picture in paginatedPictures.Items)
        {
            var user = userRepository.GetUserById(picture.UserId);
            pictureResults.Add(mapper.Map<PictureResult>((picture, user)));
        }

        return new PaginatedList<PictureResult>(
            pictureResults,
            paginatedPictures.TotalCount,
            paginatedPictures.PageNumber,
            paginatedPictures.PageSize);
    }
}