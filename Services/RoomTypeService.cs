using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using NetDapperWebApi.Common.Interfaces;
using NetDapperWebApi.DTO;
using NetDapperWebApi.DTO.Creates;
using NetDapperWebApi.DTO.Updates;
using NetDapperWebApi.Entities;
using NetDapperWebApi.Models;
using Newtonsoft.Json;
using WebApi.Context;

namespace NetDapperWebApi.Services
{
    public class RoomTypeService : IRoomTypeService
    {
        private readonly IDbConnection _db;
        private readonly ILogger<RoomTypeService> _logger;
        private readonly IFileUploadService _fileUploadService;

        public RoomTypeService(ILogger<RoomTypeService> logger, IDbConnection db, IFileUploadService fileUploadService)
        {
            _logger = logger;
            _db = db;
            _fileUploadService = fileUploadService;
        }

        public async Task<string> AddAmenitiesToRoomTypeAsync(AddRelationsMM<int, int> dto)
        {
            // Serialize danh sách CategoryIds thành JSON
            string jsonCategories = System.Text.Json.JsonSerializer.Serialize(dto.Ids); // Ví dụ: "[1,2,3]"

            var parameters = new DynamicParameters();
            parameters.Add("@RoomTypeId", dto.EntityId, DbType.Int32);
            parameters.Add("@AmenitiesJson", jsonCategories, DbType.String);
            
            var result = await _db.QueryFirstOrDefaultAsync<string>("sp_AddRoomTypeAmenities", parameters, commandType: CommandType.StoredProcedure);

            return result;
        }

        public async Task<RoomType> CreateRoomType(CreateRoomTypeDTO dto)
        {
            string thumbnail = string.Empty;
            List<string> images = [];
            try
            {
                if (dto.Thumbnail != null)
                {
                    thumbnail = await _fileUploadService.UploadSingleFile(["uploads", "images", $"{nameof(RoomType)}s"], dto.Thumbnail);
                }
                if (dto.Images.Any())
                {
                    images = await _fileUploadService.UploadMultipleFiles(["uploads", "images", $"{nameof(RoomType)}s"], dto.Images);
                }
                var imagesJson = JsonConvert.SerializeObject(images);
                var parameters = new DynamicParameters();
              
                parameters.Add("@Name", dto.Name);
                parameters.Add("@Description", dto.Description);
                parameters.Add("@PricePerNight", dto.PricePerNight);
                parameters.Add("@NumberOfBathrooms", dto.NumberOfBathrooms);
                parameters.Add("@NumberOfBeds", dto.NumberOfBeds);
                parameters.Add("@SingleBed", dto.SingleBed);
                parameters.Add("@DoubleBed", dto.DoubleBed);
                parameters.Add("@Capacity", dto.Capacity);
                parameters.Add("@Sizes", dto.Sizes);
                parameters.Add("@Thumbnail", thumbnail);
                parameters.Add("@Images", imagesJson);

                var result = await _db.QueryFirstOrDefaultAsync<RoomType>("RoomTypes_Create", parameters, commandType: CommandType.StoredProcedure);
                return result!;
            }
            catch (Exception)
            {
                if (!string.IsNullOrEmpty(thumbnail))
                {
                    _fileUploadService.DeleteSingleFile(thumbnail);
                }
                if (images.Count != 0)
                {
                    _fileUploadService.DeleteMultipleFiles(images);
                }
                throw;
            }
        }

        public async Task<bool> DeleteRoomType(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            var result = await _db.QueryFirstOrDefaultAsync<bool>(
                "RoomTypes_Delete", parameters, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<RoomType> GetRoomType(int id, int depth)
        {
         
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            parameters.Add("@Depth", depth);
            using var multi = await _db.QueryMultipleAsync(
                 "RoomTypes_GetByID", parameters, commandType: CommandType.StoredProcedure);
            var roomType = await multi.ReadFirstOrDefaultAsync<RoomType>();

            if (depth >= 1)
            {
                roomType.Rooms = (await multi.ReadAsync<Room>()).ToList();


            }

            return roomType;
        }

        public async Task<PaginatedResult<RoomType>> GetRoomTypes(PaginationModel paginationModel)
        {
            var multi = await _db.QueryMultipleAsync(
                "RoomTypes_GetAll",
                new
                {
                    paginationModel.PageNumber,
                    paginationModel.PageSize,
                    paginationModel.Depth,
                    paginationModel.Search
                },
                commandType: CommandType.StoredProcedure
            );

            // Lấy tổng số bản ghi
            int totalCount = await multi.ReadSingleAsync<int>();

            // Lấy danh sách RoomTypes đã phân trang
            var roomTypes = (await multi.ReadAsync<RoomType>()).ToList();

            if (paginationModel.Depth >= 1)
            {
                var rooms = (await multi.ReadAsync<Room>()).ToList();
                foreach (var rt in roomTypes)
                {
                    rt.Rooms = [.. rooms.Where(r => r.RoomTypeId == rt.Id)];
                }

            }


            return new PaginatedResult<RoomType>(roomTypes, totalCount, paginationModel.PageSize, paginationModel.PageNumber);
        }



        public async Task<RoomType> UpdateRoomType(int id, UpdateRoomTypeDTO dto)
        {
            var existingRoomType = await GetRoomType(id, 0) ?? throw new Exception("RoomType not found");

            List<string> uploadedImagesUrls = new();
            string thumbnail = existingRoomType.Thumbnail;

            try
            {
                // ✅ Cập nhật Thumbnail nếu có
                if (dto.Thumbnail != null)
                {
                    if (!string.IsNullOrEmpty(thumbnail))
                    {
                        _fileUploadService.DeleteSingleFile(thumbnail);
                    }

                    thumbnail = await _fileUploadService.UploadSingleFile(
                        ["uploads", "images", $"{nameof(RoomType)}s"], dto.Thumbnail);
                    uploadedImagesUrls.Add(thumbnail);
                }

                // ✅ Xử lý danh sách ảnh
                List<string> currentImages = existingRoomType.ImageList ?? new();
                List<string> keptImages = dto.KeptImages ?? new();
                List<string> updatedImageList;

                // ❌ Nếu không truyền ảnh giữ lại -> Xóa tất cả ảnh cũ
                if (!keptImages.Any())
                {
                    _fileUploadService.DeleteMultipleFiles(currentImages);
                    updatedImageList = new();
                }
                else
                {
                    // ✅ Xóa ảnh không nằm trong danh sách keptImages
                    var imagesToDelete = currentImages.Except(keptImages).ToList();
                    if (imagesToDelete.Any())
                    {
                        _fileUploadService.DeleteMultipleFiles(imagesToDelete);
                    }

                    // ✅ Giữ lại ảnh cũ cần thiết
                    List<string> retainedImages = currentImages.Intersect(keptImages).ToList();
                    updatedImageList = retainedImages;
                }

                // ✅ Tải lên ảnh mới (nếu có)
                if (dto?.Images?.Any() == true)
                {
                    var newImages = await _fileUploadService.UploadMultipleFiles(
                        ["uploads", "images", $"{nameof(RoomType)}s"], dto.Images);
                    uploadedImagesUrls.AddRange(newImages);
                    updatedImageList.AddRange(newImages);
                }

                // ✅ Cập nhật vào DB
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);
                parameters.Add("@Name", dto.Name);
                parameters.Add("@Description", dto.Description);
                parameters.Add("@PricePerNight", dto.PricePerNight);
                parameters.Add("@NumberOfBathrooms", dto.NumberOfBathrooms);
                parameters.Add("@NumberOfBeds", dto.NumberOfBeds);
                parameters.Add("@SingleBed", dto.SingleBed);
                parameters.Add("@DoubleBed", dto.DoubleBed);
                parameters.Add("@Capacity", dto.Capacity);
                parameters.Add("@Sizes", dto.Sizes);
                parameters.Add("@Thumbnail", thumbnail);
                parameters.Add("@Images", JsonConvert.SerializeObject(updatedImageList));

                var result = await _db.QueryFirstOrDefaultAsync<RoomType>(
                    "RoomTypes_Update", parameters, commandType: CommandType.StoredProcedure);

                return result;
            }
            catch
            {
                // ✅ Nếu lỗi, rollback ảnh đã tải lên nhưng chưa lưu vào DB
                if (uploadedImagesUrls.Any())
                {
                    _fileUploadService.DeleteMultipleFiles(uploadedImagesUrls);
                }

                throw;
            }
        }


    }
}