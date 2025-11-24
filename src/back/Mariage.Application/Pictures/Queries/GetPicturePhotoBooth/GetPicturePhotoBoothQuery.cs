using ErrorOr;
using Mariage.Application.Pictures.Common;
using Mariage.Domain.PictureAggregate;
using Mariage.Domain.UserAggregate.ValueObjects;
using MediatR;

namespace Mariage.Application.Pictures.Queries;

public record GetPicturePhotoBoothQuery(
    ): IRequest<ErrorOr<List<PicturePhotoBoothResult>>>;