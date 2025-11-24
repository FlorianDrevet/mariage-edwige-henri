using ErrorOr;
using MapsterMapper;
using Mariage.Application.Common.Interfaces.Persistence;
using Mariage.Application.Pictures.Common;
using Mariage.Domain.PictureAggregate;
using MediatR;

namespace Mariage.Application.Pictures.Commands.CreatePicture;

public class CreatePictureCommandHandler(IPictureRepository pictureRepository, IUserRepository userRepository, IMapper mapper)
    : IRequestHandler<CreatePictureCommand, ErrorOr<PictureResult>>
{
    public async Task<ErrorOr<PictureResult>> Handle(CreatePictureCommand request, CancellationToken cancellationToken)
    {
        var picture = Picture.Create(request.UrlPicture, request.UserId);
        pictureRepository.AddPicture(picture);
        
        var user = userRepository.GetUserById(request.UserId);
        
        return mapper.Map<PictureResult>((picture, user));
    }
}