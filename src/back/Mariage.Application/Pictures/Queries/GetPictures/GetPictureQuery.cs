using ErrorOr;
using Mariage.Application.Common.Models;
using Mariage.Application.Pictures.Common;
using MediatR;

namespace Mariage.Application.Pictures.Queries;

public record GetPictureQuery(
    int PageNumber, int PageSize
    ): IRequest<ErrorOr<PaginatedList<PictureResult>>>;