namespace AvenaWeb.Services;

public interface IImageStorageService
{
    /// <summary>
    /// Uploads a file to blob storage and returns the public URL to store on the entity.
    /// </summary>
    Task<string> UploadAsync(Stream content, string fileName, string contentType);
}
