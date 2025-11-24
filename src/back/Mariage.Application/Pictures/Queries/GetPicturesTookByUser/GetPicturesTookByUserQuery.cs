using ErrorOr;
using Mariage.Application.Pictures.Common;
using Mariage.Domain.UserAggregate.ValueObjects;
using MediatR;

namespace Mariage.Application.Pictures.Queries.GetPicturesTookByUser;

public record GetPicturesTookByUserQuery(
    int Page, int PageSize, UserId UserId
) : IRequest<ErrorOr<List<PictureResult>>>;