using ErrorOr;
using Mariage.Domain.PictureAggregate.ValueObject;
using Mariage.Domain.UserAggregate.ValueObjects;
using MediatR;

namespace Mariage.Application.Pictures.Commands.RemovePicture;

public record RemovePictureCommand(
   PictureId PictureId, UserId UserId
) : IRequest<ErrorOr<bool>>;