using ErrorOr;
using Mariage.Application.Pictures.Common;
using MediatR;

namespace Mariage.Application.Pictures.Queries;

public record GetPicturePhotographQuery(
) : IRequest<ErrorOr<List<PicturePhotographResult>>>;