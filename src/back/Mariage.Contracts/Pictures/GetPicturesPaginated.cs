namespace Mariage.Contracts.Pictures;

public record GetPicturesPaginated(
    int Page,
    int PageSize);