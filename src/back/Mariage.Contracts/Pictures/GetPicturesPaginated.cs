namespace Mariage.Contracts.Pictures;

public record GetPicturesPaginated(
    int PageNumber,
    int PageSize);