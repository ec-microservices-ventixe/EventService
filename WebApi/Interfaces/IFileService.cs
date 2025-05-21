namespace WebApi.Interfaces;

public interface IFileService
{
    Task<string> UploadFileAsync(IFormFile file);
}
