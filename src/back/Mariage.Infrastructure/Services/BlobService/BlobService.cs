using Azure.Storage.Blobs;
using Mariage.Application.Common.Interfaces.Services;
using Microsoft.Extensions.Options;

namespace Mariage.Infrastructure.Services.BlobService;

public class BlobService(
    IOptions<BlobSettings> blobStorageSettings)
    : IBlobService
{
    private readonly BlobServiceClient _blobServiceClient =
        new BlobServiceClient(blobStorageSettings.Value.ConnectionString);

    private readonly BlobServiceClient _blobServiceClientPictures =
        new BlobServiceClient(blobStorageSettings.Value.ConnectionStringPictures);

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(blobStorageSettings.Value.ContainerName);
        var blobClient = containerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(fileStream, true);
        return blobClient.Uri.ToString();
    }

    public async Task<string> UploadPictureAsync(Stream fileStream, string fileName)
    {
        var containerClient =
            _blobServiceClientPictures.GetBlobContainerClient(blobStorageSettings.Value.ContainerPicturesName);
        var blobClient = containerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(fileStream, true);
        return blobClient.Uri.ToString();
    }

    public Task<string> DeleteFileAsync(string fileName)
    {
        throw new NotImplementedException();
    }

    public async Task<List<string>> GetAllFilesPhotoBoothAsync()
    {
        return await GetAllFilesBlobAsync(blobStorageSettings.Value.ContainerPicturePhotoBoothsName);
    }

    public async Task<List<string>> GetAllFilesPhotgraphAsync()
    {
        return await GetAllFilesBlobAsync(blobStorageSettings.Value.ContainerPicturePhotographName);
    }

    private async Task<List<string>> GetAllFilesBlobAsync(string nameContainer)
    {
        var containerClient = _blobServiceClientPictures.GetBlobContainerClient(nameContainer);
        var blobLinks = new List<string>();

        await foreach (var blobItem in containerClient.GetBlobsAsync())
        {
            var blobClient = containerClient.GetBlobClient(blobItem.Name);
            blobLinks.Add(blobClient.Uri.ToString());
        }

        return blobLinks;
    }
}