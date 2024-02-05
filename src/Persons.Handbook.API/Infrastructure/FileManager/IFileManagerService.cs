namespace Persons.Handbook.API.Infrastructure.FileManager;

public interface IFileManagerService
{
    Task<string> UploadImageAsync(IFormFile file);
    void DeleteImage(string imageUrl);

}