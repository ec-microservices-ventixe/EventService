using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using WebApi.Interfaces;

namespace WebApi.Services;
public class AzureFilesService(IConfiguration config) : IFileService
{
    private readonly BlobContainerClient _blobContainerClient = new(config["AzureBlogStorage:ConnectionString"], config["AzureBlogStorage:ContainerName"]);
    public async Task<string> UploadFileAsync(IFormFile file)
    {
        if (file is null || file.Length == 0)
            return null!;

        var fileExtension = Path.GetExtension(file.Name);
        var fileName = $"{Guid.NewGuid()}{fileExtension}";

        string contentType = !string.IsNullOrEmpty(file.ContentType) ? file.ContentType : "application/octet-stream";

        if ((contentType == "application/octet-stream" || string.IsNullOrEmpty(contentType)) && fileExtension.Equals(".svg", StringComparison.OrdinalIgnoreCase))
            contentType = "image/svg+xml";

        BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);
        var uploadOptions = new BlobUploadOptions {  HttpHeaders = new BlobHttpHeaders { ContentType = contentType } };

        using var stram = file.OpenReadStream();
        await blobClient.UploadAsync(stram, uploadOptions);

        return blobClient.Uri.ToString();
    }
}
