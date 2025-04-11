

namespace NetDapperWebApi.Common.Interfaces
{
    public interface IFileUploadService
    {
        Task<string> UploadSingleFile(string[] destination, IFormFile file);
        Task<List<string>> UploadMultipleFiles(string[] destination, List<IFormFile> files);
        bool DeleteSingleFile(string filePath);
        bool DeleteMultipleFiles(List<string> filePaths);
    }
}