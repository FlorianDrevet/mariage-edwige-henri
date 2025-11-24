using ErrorOr;
using MapsterMapper;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Application.Pictures.Common;
using Mariage.Domain.PictureAggregate;
using MediatR;

namespace Mariage.Application.Pictures.Queries;

public class GetPictureQueryHandler(IPictureRepository pictureRepository, IUserRepository userRepository, IMapper mapper)
    : IRequestHandler<GetPictureQuery, ErrorOr<List<PictureResult>>>
{
    public async Task<ErrorOr<List<PictureResult>>> Handle(GetPictureQuery request, CancellationToken cancellationToken)
    {
        var pictures = pictureRepository.GetPictures(request.Page, request.PageSize);
        
        List<PictureResult> pictureResults = new();
        foreach (var picture in pictures)
        {
            var user = userRepository.GetUserById(picture.UserId);
            pictureResults.Add(mapper.Map<PictureResult>((picture, user)));
        }

        return pictureResults;
    }
}