
using System.Data;
using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;
using NetDapperWebApi.Common.Interfaces;
using NetDapperWebApi.DTO;
using NetDapperWebApi.DTO.Creates;
using NetDapperWebApi.DTO.Updates;
using NetDapperWebApi.Entities;

namespace NetDapperWebApi.Services
{
    public class ImageService : IImageService
    {
        private readonly IDbConnection _db;
        private readonly ILogger<ImageService> _logger;
        private readonly IFileUploadService _fileUploadService;

        public ImageService(IDbConnection db, IFileUploadService fileUploadService, ILogger<ImageService> logger)
        {
            _db = db;
            _fileUploadService = fileUploadService;
            _logger = logger;
        }

        public async Task<List<Image>> BulkInsertImagesAsync(CreateMutipleImage request)
        {
            var urls = new List<string>();
            try
            {
                if (request.Images == null || request.Images.Count == 0)
                {
                    throw new Exception();
                }


                // Tạo DataTable tương ứng với dbo.ImageListType
                var table = new DataTable();
                table.Columns.Add("EntityType", typeof(string));
                table.Columns.Add("EntityId", typeof(int));
                table.Columns.Add("ImageUrl", typeof(string));
                table.Columns.Add("IsThumbnail", typeof(bool));
                table.Columns.Add("SortOrder", typeof(int));
                // Upload ảnh và lấy URL
                urls = await _fileUploadService.UploadMultipleFiles(["uploads", "images", request.EntityType], request.Images);
                // 2. Map dữ liệu từ DTO sang DataTable
                // Giả định validate đã đảm bảo Images.Count == SortOrder.Count và thứ tự khớp nhau.
                for (int i = 0; i < urls.Count; i++)
                {
                    // Ở đây IsThumbnail mặc định là false, thay đổi nếu cần
                    table.Rows.Add(request.EntityType, request.EntityId, urls[i], false, request.SortOrder[i]);
                }
                // 3. Dùng Dapper để gọi stored procedure với table-valued parameter
                var parameters = new DynamicParameters();
                parameters.Add("@ImageList", table.AsTableValuedParameter("dbo.ImageListType"));

                var result = await _db.QueryAsync<Image>("usp_Image_BulkInsert", parameters, commandType: CommandType.StoredProcedure);
                return [.. result];



            }
            catch (Exception ex)
            {
                if (urls.Any())
                {
                    _fileUploadService.DeleteMultipleFiles(urls);
                }

                // Log lỗi nếu cần
                _logger.LogError(ex, "Lỗi khi thực hiện BulkInsertImagesAsync");

            
                throw;
            }


        }

        public async Task<bool> DeleteImageAsync(int imageId)
        {
            var deleted = await _db.QueryFirstOrDefaultAsync<bool>(
                    "usp_Image_Delete",
                    new { Id = imageId },
                    commandType: CommandType.StoredProcedure
                );
            return deleted;
        }

        public async Task<List<Image>> GetImagesByEntityAsync(string entityType, int entityId)
        {

            var images = await _db.QueryAsync<Image>(
                "usp_Image_GetByEntity",
                new { EntityType = entityType, EntityId = entityId },
                commandType: CommandType.StoredProcedure
            );
            return [.. images];
        }

        public Task<Image> UpdateImageAsync(int imageId, ImageUpdateDTO request)
        {
            return null;
        }

        public async Task<Image> UploadImagesAsync(ImageCreateDTO dto)
        {
            var newImage = string.Empty;
            try
            {
                newImage = await _fileUploadService.UploadSingleFile(["uploads", "images", dto.EntityType], dto.Image);
                var parameters = new DynamicParameters();
                parameters.Add("@EntityType", dto.EntityType);
                parameters.Add("@EntityId", dto.EntityId);
                parameters.Add("@ImageUrl", newImage);
                parameters.Add("@IsThumbnail", dto.IsThumbnail);
                parameters.Add("@SortOrder", dto.SortOrder);
                var multi = await _db.QueryFirstOrDefaultAsync<Image>("usp_Image_Insert", parameters, commandType: CommandType.StoredProcedure);
                return multi;
            }
            catch (Exception)
            {
                if (!string.IsNullOrEmpty(newImage))
                {
                    _fileUploadService.DeleteSingleFile(newImage);
                }
                throw;
            }

        }


    }
}