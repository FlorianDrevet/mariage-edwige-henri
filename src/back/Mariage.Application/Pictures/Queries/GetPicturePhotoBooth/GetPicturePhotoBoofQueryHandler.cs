using ErrorOr;
using MapsterMapper;
using Mariage.Application.Common.Interfaces.Services;
using Mariage.Application.Pictures.Common;
using MediatR;

namespace Mariage.Application.Pictures.Queries.GetPicturePhotoBooth;

public class GetPicturePhotoBoofQueryHandler(IMapper mapper, IBlobService blobService)
    : IRequestHandler<GetPicturePhotoBoothQuery, ErrorOr<List<PicturePhotoBoothResult>>>
{
    public async Task<ErrorOr<List<PicturePhotoBoothResult>>> Handle(GetPicturePhotoBoothQuery request,
        CancellationToken cancellationToken)
    {
        var pictures = await blobService.GetAllFilesPhotoBoothAsync();

        List<PicturePhotoBoothResult> res = new();

        foreach (var p in pictures)
        {
            res.Add(new PicturePhotoBoothResult(p));
        }

        return res;
    }
}