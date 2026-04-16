using ErrorOr;
using Mariage.Application.Common.Models;
using Mariage.Application.Pictures.Common;
using Mariage.Domain.UserAggregate.ValueObjects;
using MediatR;

namespace Mariage.Application.Pictures.Queries.GetPicturesTookByUser;

public record GetPicturesTookByUserQuery(
    int PageNumber, int PageSize, UserId UserId
) : IRequest<ErrorOr<PaginatedList<PictureResult>>>;