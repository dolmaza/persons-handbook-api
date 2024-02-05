using Microsoft.Extensions.Options;
using Persons.Handbook.Infrastructure.Configs;

namespace Persons.Handbook.API.Infrastructure.FileManager;

public class FileManagerService : IFileManagerService
{
    private readonly UploadFoldersConfig _uploadFoldersConfig;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public FileManagerService(IOptions<UploadFoldersConfig> uploadFoldersConfig, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
    {
        _webHostEnvironment = webHostEnvironment;
        _httpContextAccessor = httpContextAccessor;
        _uploadFoldersConfig = uploadFoldersConfig.Value;
    }

    public async Task<string> UploadImageAsync(IFormFile file)
    {
        var fileExtension = Path.GetExtension(file.FileName);
        var fileName = $"{Guid.NewGuid()}{fileExtension}";
        var folderName = _uploadFoldersConfig.ImagesFolder;

        await UploadFile(file, folderName, fileName);

        return FileUrl(folderName, fileName);
    }

    public void DeleteImage(string imageUrl)
    {
        if (string.IsNullOrEmpty(imageUrl))
        {
            throw new Exception("Url can't be null or empty");
        }

        var fileName = imageUrl.Split("/").Last();

        var folderName = _uploadFoldersConfig.ImagesFolder;

        DeleteFile(folderName, fileName);
    }

    private async Task UploadFile(IFormFile file, string? folderName, string? fileName)
    {
        var fileInfo = new FileInfo($"{_webHostEnvironment.WebRootPath}/{folderName}/{fileName}");

        await using var stream = fileInfo.Create();
        await file.CopyToAsync(stream);
    }

    private void DeleteFile(string? folderName, string? fileName)
    {
        var fileInfo = new FileInfo($"{_webHostEnvironment.WebRootPath}/{folderName}/{fileName}");
        fileInfo.Delete();
    }

    private string FileUrl(string? folderName, string? fileName) => $"{_httpContextAccessor.HttpContext?.Request.Host.ToString()}/{folderName}/{fileName}";
}