using ErrorOr;
using MapsterMapper;
using Mariage.Application.Common.Interfaces.Services;
using Mariage.Application.Pictures.Common;
using MediatR;

namespace Mariage.Application.Pictures.Queries.GetPicturePhotoBooth;

public class GetPicturePhotographQueryHandler(IMapper mapper, IBlobService blobService)
    : IRequestHandler<GetPicturePhotographQuery, ErrorOr<List<PicturePhotographResult>>>
{
    public async Task<ErrorOr<List<PicturePhotographResult>>> Handle(GetPicturePhotographQuery request,
        CancellationToken cancellationToken)
    {
        var pictures = await blobService.GetAllFilesPhotgraphAsync();

        List<PicturePhotographResult> res = new();

        foreach (var p in pictures)
        {
            res.Add(new PicturePhotographResult(p));
        }

        return res;
    }
}