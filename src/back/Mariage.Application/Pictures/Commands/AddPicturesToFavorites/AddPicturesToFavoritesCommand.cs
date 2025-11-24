using ErrorOr;
using Mariage.Domain.PictureAggregate.ValueObject;
using Mariage.Domain.UserAggregate.ValueObjects;
using MediatR;

namespace Mariage.Application.Pictures.Commands.AddPicturesToFavorites;

public record AddPicturesToFavoritesCommand(
    PictureId PictureId, UserId UserId
) : IRequest<ErrorOr<bool>>;