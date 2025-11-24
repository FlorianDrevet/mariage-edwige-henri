namespace Mariage.Application.Common.Interfaces.Services;

public interface IBlobService
{
    public Task<string> UploadFileAsync(Stream fileStream, string fileName);
    public Task<string> UploadPictureAsync(Stream fileStream, string fileName);
    public Task<string> DeleteFileAsync(string fileName);
    public Task<List<string>> GetAllFilesPhotoBoothAsync();
    public Task<List<string>> GetAllFilesPhotgraphAsync();
}