using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace AvenaWeb.Services;

/// <summary>
/// Stores uploaded images in Azure Blob Storage instead of the local disk,
/// so files survive container restarts/redeploys and app scale-out.
/// </summary>
public class BlobImageStorageService : IImageStorageService
{
    private readonly BlobContainerClient _containerClient;

    public BlobImageStorageService(IConfiguration configuration)
    {
        var connectionString = configuration["BlobStorage:ConnectionString"]
            ?? throw new InvalidOperationException(
                "Missing configuration 'BlobStorage:ConnectionString'. " +
                "Set it in appsettings.json (dev) or as an App Service " +
                "application setting named BlobStorage__ConnectionString (prod).");

        var containerName = configuration["BlobStorage:ContainerName"] ?? "images";

        var serviceClient = new BlobServiceClient(connectionString);
        _containerClient = serviceClient.GetBlobContainerClient(containerName);

        // Make sure the container exists and its blobs are publicly readable,
        // since images are embedded directly in <img>/background-image on public pages.
        _containerClient.CreateIfNotExists(PublicAccessType.Blob);
    }

    public async Task<string> UploadAsync(Stream content, string fileName, string contentType)
    {
        var blobClient = _containerClient.GetBlobClient(fileName);

        await blobClient.UploadAsync(
            content,
            new BlobHttpHeaders { ContentType = contentType });

        return blobClient.Uri.ToString();
    }
}
