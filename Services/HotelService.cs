using System.Data;
using System.Text.Json.Serialization;
using Dapper;
using NetDapperWebApi.Common.Interfaces;
using NetDapperWebApi.DTO;
using NetDapperWebApi.DTO.Creates;
using NetDapperWebApi.DTO.Updates;
using NetDapperWebApi.Entities;
using NetDapperWebApi.Models;
using Newtonsoft.Json;

namespace NetDapperWebApi.Services
{
    public class HotelService : IHotelService
    {
        private readonly IDbConnection _db;

        private readonly ILogger<HotelService> _logger;
        private readonly IFileUploadService _fileUploadService;
        public HotelService(IDbConnection db, ILogger<HotelService> logger, IFileUploadService fileUploadService)
        {
            _db = db;
            _logger = logger;
            _fileUploadService = fileUploadService;
        }

        public async Task<string> AddAmenitiesToHotelAsync(AddRelationsMM<int, int> dto)
        {

            // Serialize danh sách  thành JSON
            string jsonAmenities = System.Text.Json.JsonSerializer.Serialize(dto?.Ids); // Ví dụ: "[1,2,3]"
            var parameters = new DynamicParameters();
            parameters.Add("@HotelId", dto.EntityId);
            parameters.Add("@AmenitiesJson", jsonAmenities);
            var result = await _db.QueryFirstOrDefaultAsync<string>("sp_AddHotelAmenities", parameters, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<Hotel> CreateHotel(CreateHotelDTO hotel)
        {
            var thumbnail = string.Empty;
            List<string> images = [];
            try
            {
                if (hotel.Thumbnail != null)
                {
                    thumbnail = await _fileUploadService.UploadSingleFile(["uploads", "images", $"{nameof(Hotel)}s"], hotel.Thumbnail);
                }
                if (hotel.Images.Any())
                {
                    images = await _fileUploadService.UploadMultipleFiles(["uploads", "images", $"{nameof(Hotel)}s"], hotel.Images);
                }
                var imagesJson = JsonConvert.SerializeObject(images);
                var parameters = new DynamicParameters();
                parameters.Add("@Name", hotel.Name);
                parameters.Add("@Address", hotel.Address);
                parameters.Add("@Quantity", hotel.Quantity);
                parameters.Add("@Description", hotel.Description);
                parameters.Add("@Phone", hotel.Phone);
                parameters.Add("@Location", hotel.Location);
                parameters.Add("@Email", hotel.Email);
                parameters.Add("@Thumbnail", thumbnail);
                parameters.Add("@Images", imagesJson);
                parameters.Add("@Stars", hotel.Stars);
                parameters.Add("@Floor", hotel.Floor);
                parameters.Add("@CheckinTime", hotel.CheckinTime);
                parameters.Add("@CheckoutTime", hotel.CheckoutTime);


                var result = await _db.QueryFirstOrDefaultAsync<Hotel>(
                    "Hotels_Create", parameters, commandType: CommandType.StoredProcedure);
                return result;
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

        public async Task<bool> DeleteHotel(int id)
        {
            var existingHotel = await GetHotel(id, 0);
            if (existingHotel == null)
            {
                return false; //  Nếu không tìm thấy khách sạn, trả về false
            }

            //  Xóa hình ảnh trước khi xóa khách sạn
            if (!string.IsNullOrEmpty(existingHotel.Thumbnail))
            {
                _fileUploadService.DeleteSingleFile(existingHotel.Thumbnail);
            }

            if (existingHotel.ImageList?.Any() == true)
            {
                _fileUploadService.DeleteMultipleFiles(existingHotel.ImageList);
            }

            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            var result = await _db.ExecuteAsync("Hotels_Delete", parameters, commandType: CommandType.StoredProcedure);

            return result == -1 ? true : false; //  Trả về true nếu có dòng bị ảnh hưởng (xóa thành công)
        }


        public async Task<PaginatedResult<Hotel>> GetAllHotels(PaginationModel paginationModel)
        {

            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PageNumber", paginationModel.PageNumber);
                parameters.Add("@PageSize", paginationModel.PageSize);
                parameters.Add("@Depth", paginationModel.Depth);
                parameters.Add("@Search", paginationModel.Search);

                using var multi = await _db.QueryMultipleAsync(
                  "Hotels_GetAll", parameters, commandType: CommandType.StoredProcedure);
                // Lấy tổng số bản ghi
                int totalCount = await multi.ReadSingleAsync<int>();

                // Lấy danh sách khách sạn (HotelWithRooms)
                var hotels = (await multi.ReadAsync<Hotel>()).ToList();

                // Nếu Depth >= 1, ta đọc thêm danh sách phòng
                if (paginationModel.Depth >= 1)
                {
                    var roomTypes = (await multi.ReadAsync<RoomType>()).ToList();
                    foreach (var hotel in hotels)
                    {
                        hotel.RoomTypes = roomTypes.Where(rt => rt.HotelId == hotel.Id).ToList();
                    }

                }
                // if (paginationModel.Depth >= 2)
                // {
                //     var rooms = (await multi.ReadAsync<Room>()).ToList();
                //     foreach (var hotel in hotels)
                //     {
                //         foreach (var roomType in hotel.RoomTypes)
                //         {
                //             roomType.Rooms = rooms.Where(r => r.RoomTypeId == roomType.Id).ToList();
                //         }
                //     }

                // }

                return new PaginatedResult<Hotel>(hotels, totalCount, paginationModel.PageNumber, paginationModel.PageSize);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error getting all hotels");
                throw ex;
            }

        }

        public async Task<Hotel> GetHotel(int id, int depth)
        {
            var parameters = new { Id = id, Depth = depth };

            using var multi = await _db.QueryMultipleAsync("Hotels_GetByID", parameters, commandType: CommandType.StoredProcedure);

            // Lấy thông tin khách sạn
            var hotel = await multi.ReadSingleOrDefaultAsync<Hotel>();
            if (hotel == null) return null; // Nếu không tìm thấy thì trả về null

            // Nếu depth >= 1, lấy danh sách RoomTypes + Tiện ích của Hotel
            List<Amenities> hotelAmenities = [];
            List<RoomType> roomTypes = [];

            if (depth >= 1)
            {
                hotelAmenities = [.. await multi.ReadAsync<Amenities>()];
                roomTypes = [.. await multi.ReadAsync<RoomType>()];
                hotel.RoomTypes = roomTypes;
                hotel.Amenities = hotelAmenities;
            }

            // Nếu depth >= 2, lấy danh sách Rooms + Tiện ích của RoomTypes
            if (depth >= 2)
            {

                var rooms = (await multi.ReadAsync<Room>()).ToList(); // Rooms của khách sạn
                // var roomAmenities=(await multi.ReadAsync<>)
                foreach (var roomType in hotel.RoomTypes)
                {

                    roomType.Rooms = rooms.Where(r => r.RoomTypeId == roomType.Id).ToList();
                }
            }

            return hotel;
        }

        public async Task<Hotel> GetHotelByName(string name)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Name", name);
            using var multi = await _db.QueryMultipleAsync("Hotel_GetByName", parameters, commandType: CommandType.StoredProcedure);
            var hotel = await multi.ReadFirstOrDefaultAsync<Hotel>();
            var amenities = (await multi.ReadAsync<Amenities>()).ToList();
            var roomTypes = (await multi.ReadAsync<RoomType>()).ToList();
            if (hotel != null)
                hotel.RoomTypes = roomTypes;
            hotel.Amenities = amenities;
            return hotel!;
        }

        public async Task<Hotel> UpdateHotel(int id, UpdateHotelDTO dto)
        {
            var existingHotel = await GetHotel(id, 0) ?? throw new Exception("Hotel not found");

            List<string> uploadedImagesUrls = new();
            string thumbnail = existingHotel.Thumbnail;

            try
            {
                //  Cập nhật Thumbnail nếu có
                if (dto.Thumbnail != null)
                {
                    if (!string.IsNullOrEmpty(thumbnail))
                    {
                        _fileUploadService.DeleteSingleFile(thumbnail);
                    }

                    thumbnail = await _fileUploadService.UploadSingleFile(["uploads", "images", $"{nameof(Hotel)}s"], dto.Thumbnail);
                    uploadedImagesUrls.Add(thumbnail);
                }

                //  Xử lý danh sách ảnh
                List<string> currentImages = existingHotel.ImageList ?? new();
                List<string> keptImages = dto.KeptImages ?? new();
                List<string> updatedImageList;

                // Nếu không truyền ảnh giữ lại -> Xóa tất cả ảnh cũ
                if (!keptImages.Any())
                {
                    _fileUploadService.DeleteMultipleFiles(currentImages);
                    updatedImageList = new();
                }
                else
                {
                    //  Xóa ảnh không nằm trong danh sách keptImages
                    var imagesToDelete = currentImages.Except(keptImages).ToList();
                    if (imagesToDelete.Any())
                    {
                        _fileUploadService.DeleteMultipleFiles(imagesToDelete);
                    }

                    //  Giữ lại ảnh cũ cần thiết
                    List<string> retainedImages = currentImages.Intersect(keptImages).ToList();
                    updatedImageList = retainedImages;
                }

                //  Tải lên ảnh mới (nếu có)
                if (dto?.Images?.Any() == true)
                {
                    var newImages = await _fileUploadService.UploadMultipleFiles(["uploads", "images", $"{nameof(Hotel)}s"], dto.Images);
                    uploadedImagesUrls.AddRange(newImages); // Thêm vào danh sách rollback nếu lỗi
                    updatedImageList.AddRange(newImages);
                }

                //  Cập nhật vào DB
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);
                parameters.Add("@Name", dto.Name);
                parameters.Add("@Quantity", dto.Quantity);
                parameters.Add("@Description", dto.Description);
                parameters.Add("@Location", dto.Location);
                parameters.Add("@Address", dto.Address);
                parameters.Add("@Phone", dto.Phone);
                parameters.Add("@Email", dto.Email);
                parameters.Add("@Thumbnail", thumbnail);
                parameters.Add("@Images", JsonConvert.SerializeObject(updatedImageList));
                parameters.Add("@Stars", dto.Stars);
                parameters.Add("@Floor", dto.Floor);
                parameters.Add("@CheckinTime", dto.CheckinTime);
                parameters.Add("@CheckoutTime", dto.CheckoutTime);

                var result = await _db.QueryFirstOrDefaultAsync<Hotel>(
                    "Hotels_Update", parameters, commandType: CommandType.StoredProcedure);

                return result;
            }
            catch
            {
                //  Nếu lỗi, rollback ảnh đã tải lên nhưng chưa lưu vào DB
                if (uploadedImagesUrls.Any())
                {
                    _fileUploadService.DeleteMultipleFiles(uploadedImagesUrls);
                }

                throw;
            }
        }

    }

}