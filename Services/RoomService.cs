
using System.Data;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Dapper;
using NetDapperWebApi.Common.Interfaces;
using NetDapperWebApi.DTO;
using NetDapperWebApi.DTO.Creates.Rooms;
using NetDapperWebApi.DTO.Updates;
using NetDapperWebApi.Entities;
using NetDapperWebApi.Models;
using Newtonsoft.Json;

namespace NetDapperWebApi.Services
{
    public class RoomService : IRoomService
    {
        private readonly IDbConnection _db;
        private readonly IFileUploadService _fileService;
        private readonly ILogger<RoomService> _logger;

        public RoomService(IDbConnection db, ILogger<RoomService> logger, IFileUploadService fileService)
        {
            _db = db;
            _logger = logger;
            _fileService = fileService;
        }





        public async Task<Room> CreateRoom(CreateRoomDTO room)
        {
            var thumbnail = string.Empty;

            var images = new List<string>();

            var newImages = new List<string>();

            try
            {
                if (room.Thumbnail != null)
                {
                    thumbnail = room.Thumbnail != null ? await _fileService.UploadSingleFile(["uploads", "images", $"{nameof(Room)}s"], room.Thumbnail) : null;

                }
                if (room.Images != null && room.Images.Any())
                {
                    images = await _fileService.UploadMultipleFiles(["uploads", "images", $"{nameof(Room)}s"], room.Images);
                }
                var imagesJson = JsonConvert.SerializeObject(images);
                var parameters = new DynamicParameters();
                parameters.Add("@RoomTypeId", room.RoomTypeId);
                parameters.Add("@RoomNumber", room.RoomNumber);
                parameters.Add("@Thumbnail", thumbnail);
                parameters.Add("@Images", imagesJson);

                parameters.Add("@Floor", room.Floor);
                parameters.Add("@Status", room.Status.ToString());


                var result = await _db.QueryFirstOrDefaultAsync<Room>(
                    "Rooms_Create", parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(thumbnail))
                {
                    _fileService.DeleteSingleFile(thumbnail);
                }
                if (images.Count >= 0)
                {
                    _fileService.DeleteMultipleFiles(images);
                }
                throw;
            }
        }

        public async Task<bool> DeleteRoom(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            var result = await _db.QueryFirstOrDefaultAsync<bool>(
                "Rooms_Delete", parameters, commandType: CommandType.StoredProcedure);
            return result;
        }
        public async Task<Room> GetRoom(int id, int depth)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            parameters.Add("@Depth", depth);
            Room? room;

            using var multi = await _db.QueryMultipleAsync(
                  "Rooms_GetByID", parameters, commandType: CommandType.StoredProcedure);

            room = await multi.ReadSingleAsync<Room>();



            if (depth >= 1)
            {

                room.RoomType = await multi.ReadSingleAsync<RoomType>();
                room.Bookings = (await multi.ReadAsync<Booking>()).ToList();

        
            }

            return room;

        }


        public async Task<PaginatedResult<Room>> GetRooms(PaginationModel paginationModel)
        {
            var parameters = new
            {
                PageNumber = paginationModel.PageNumber,
                PageSize = paginationModel.PageSize,
                Depth = paginationModel.Depth,
                Search = paginationModel.Search
            };

            using var multi = await _db.QueryMultipleAsync("Rooms_GetAll", parameters, commandType: CommandType.StoredProcedure);

            var totalCount = await multi.ReadFirstOrDefaultAsync<int>(); // Đọc TotalCount
            var rooms = (await multi.ReadAsync<Room>()).ToList(); // Đọc danh sách Rooms
                                                                  // Xử lý ImagesList cho tất cả rooms ngay lập tức, không phụ thuộc vào Depth
                                                                  //lấy roomType của từng phòng 

            if (rooms.Count != 0 && paginationModel.Depth >= 1)
            {
                var roomTypes = (await multi.ReadAsync<RoomType>()).ToList();//2 phòng

                foreach (var room in rooms)
                {
                    room.RoomType = roomTypes.Where(x => x.Id == room.RoomTypeId).FirstOrDefault();
                }
            }

            return new PaginatedResult<Room>(rooms, totalCount, paginationModel.PageNumber, paginationModel.PageSize);
        }


        public async Task<Room> UpdateRoom(int id, UpdateRoomDTO room)
        {
            var existingRoom = await GetRoom(id, 0);
            if (existingRoom == null)
                throw new KeyNotFoundException("Room not found.");

            // ✅ Danh sách lưu ảnh đã tải lên để rollback nếu lỗi xảy ra
            List<string> uploadedImagesUrls = [];

            try
            {
                // ✅ Xử lý Thumbnail
                string thumbnail = existingRoom.Thumbnail;
                if (room.Thumbnail != null)
                {
                    // Xóa thumbnail cũ trước khi lưu mới
                    if (!string.IsNullOrEmpty(existingRoom.Thumbnail))
                    {
                        _fileService.DeleteSingleFile(existingRoom.Thumbnail);
                    }

                    thumbnail = await _fileService.UploadSingleFile(
                        ["uploads", "images", $"{nameof(Room)}s"], room.Thumbnail);

                    // Lưu lại để rollback nếu có lỗi
                    uploadedImagesUrls.Add(thumbnail);
                }

                // ✅ Xử lý danh sách Images
                List<string> currentImages = existingRoom.ImageList ?? new List<string>();

                // 🔹 **Bước 1: Ảnh giữ lại** (frontend gửi lên danh sách URL ảnh giữ lại)
                List<string> keptImageUrls = room.KeptImages ?? new List<string>();

                // 🔹 **Bước 2: Xóa ảnh không còn trong danh sách giữ lại**
                var imagesToDelete = currentImages.Where(img => !keptImageUrls.Contains(img)).ToList();
                _fileService.DeleteMultipleFiles(imagesToDelete);

                // 🔹 **Bước 3: Upload ảnh mới**
                List<string> finalImagesList = currentImages
                    .Where(img => keptImageUrls.Contains(img)) // Giữ ảnh cũ
                    .ToList();

                if (room.Images != null && room.Images.Any())
                {
                    var uploadedImages = await _fileService.UploadMultipleFiles(
                        ["uploads", "images", $"{nameof(Room)}s"], room.Images);

                    finalImagesList.AddRange(uploadedImages);
                    uploadedImagesUrls.AddRange(uploadedImages);
                }
                var imagesJson = JsonConvert.SerializeObject(finalImagesList);
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);

                if (room.RoomTypeId != null)
                    parameters.Add("@RoomTypeId", room.RoomTypeId);
                parameters.Add("@RoomNumber", room.RoomNumber);
                parameters.Add("@Thumbnail", thumbnail);
                parameters.Add("@Images", imagesJson);

                parameters.Add("@Floor", room.Floor);
                parameters.Add("@Status", room.Status.ToString());
                parameters.Add("@UpdatedAt", DateTime.UtcNow);

                var result = await _db.QueryFirstOrDefaultAsync<Room>(
                    "Rooms_Update", parameters, commandType: CommandType.StoredProcedure);

                return result;
            }
            catch (Exception ex)
            {
                // ✅ Nếu có lỗi, xóa tất cả ảnh mới tải lên nhưng chưa lưu vào DB
                if (uploadedImagesUrls.Any())
                {
                    _fileService.DeleteMultipleFiles(uploadedImagesUrls);
                }

                throw new Exception("Error updating room: " + ex.Message);
            }
        }

    }
}