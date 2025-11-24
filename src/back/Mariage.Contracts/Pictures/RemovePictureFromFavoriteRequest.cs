using Mariage.Domain.PictureAggregate.ValueObject;

namespace Mariage.Contracts.Pictures;

public record RemovePictureFromFavoriteRequest(
    Guid PictureId);