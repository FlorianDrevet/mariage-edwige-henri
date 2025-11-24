using ErrorOr;
using Mariage.Application.Pictures.Common;
using Mariage.Domain.PictureAggregate;
using Mariage.Domain.UserAggregate.ValueObjects;
using MediatR;

namespace Mariage.Application.Pictures.Commands.CreatePicture;

public record CreatePictureCommand(
    UserId UserId,
    string UrlPicture
    ) : IRequest<ErrorOr<PictureResult>>;