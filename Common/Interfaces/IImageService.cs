
using NetDapperWebApi.DTO;
using NetDapperWebApi.DTO.Creates;
using NetDapperWebApi.DTO.Updates;
using NetDapperWebApi.Entities;

namespace NetDapperWebApi.Common.Interfaces
{
    public interface IImageService
    {
        Task<List<Image>> GetImagesByEntityAsync(string entityType, int entityId);
        Task<Image> UploadImagesAsync(ImageCreateDTO dto);
        Task<bool> DeleteImageAsync(int imageId);
        Task<Image> UpdateImageAsync(int imageId, ImageUpdateDTO request);
        Task<List<Image>> BulkInsertImagesAsync(CreateMutipleImage request);
    }

}