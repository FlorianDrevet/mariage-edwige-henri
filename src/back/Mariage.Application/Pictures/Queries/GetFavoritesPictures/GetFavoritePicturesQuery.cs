using ErrorOr;
using Mariage.Application.Pictures.Common;
using Mariage.Domain.PictureAggregate;
using Mariage.Domain.UserAggregate.ValueObjects;
using MediatR;

namespace Mariage.Application.Pictures.Queries.GetFavoritesPictures;

public record GetFavoritePicturesQuery(
    int Page, int PageSize, UserId UserId
) : IRequest<ErrorOr<List<PictureResult>>>;