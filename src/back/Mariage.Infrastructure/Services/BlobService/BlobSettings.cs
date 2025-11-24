namespace Mariage.Infrastructure.Services.BlobService;

public class BlobSettings
{
    public const string SectionName = "BlobSettings";
    public string ConnectionString { get; init; } = null!;
    public string ContainerName { get; init; } = null!;

    public string ConnectionStringPictures { get; init; } = null!;
    public string ContainerPicturesName { get; init; } = null!;
    public string ContainerPicturePhotoBoothsName { get; init; } = null!;
    public string ContainerPicturePhotographName { get; init; } = null!;
}