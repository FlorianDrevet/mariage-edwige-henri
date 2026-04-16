using ErrorOr;
using Mariage.Application.Common.Models;
using Mariage.Application.Pictures.Common;
using Mariage.Domain.UserAggregate.ValueObjects;
using MediatR;

namespace Mariage.Application.Pictures.Queries.GetFavoritesPictures;

public record GetFavoritePicturesQuery(
    int PageNumber, int PageSize, UserId UserId
) : IRequest<ErrorOr<PaginatedList<PictureResult>>>;