
using System.Data;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Dapper;
using NetDapperWebApi_local.Common.Interfaces;
using NetDapperWebApi_local.DTO;
using NetDapperWebApi_local.DTO.Creates.Rooms;
using NetDapperWebApi_local.DTO.Updates;
using NetDapperWebApi_local.Entities;
using NetDapperWebApi_local.Models;
using NetDapperWebApi_local.DTO.Res;
using Newtonsoft.Json;

namespace NetDapperWebApi_local.Services
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


            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@RoomTypeId", room.RoomTypeId);
                parameters.Add("@RoomNumber", room.RoomNumber);
                parameters.Add("@Floor", room.Floor);
                parameters.Add("@Status", room.Status.ToString());
                parameters.Add("@CleanStatus", room.CleanStatus.ToString());
                var result = await _db.QueryFirstOrDefaultAsync<Room>(
                    "Rooms_Create", parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (Exception ex)
            {
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


        public async Task<PaginatedResult<RoomResult>> GetRooms(PaginationModel paginationModel, RoomFilters filters)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@PageNumber", paginationModel.PageNumber);
            parameters.Add("@PageSize", paginationModel.PageSize);
            parameters.Add("@Depth", paginationModel.Depth);
            parameters.Add("@Search", paginationModel.Search ?? string.Empty);
            if (filters != null)
            {
                parameters.Add("@Status", filters.Status?.ToString() ?? string.Empty);
                parameters.Add("@CleanStatus", filters.CleanStatus?.ToString() ?? string.Empty);
                parameters.Add("@IsSingleBed", filters.IsSingleBed ?? null);
                parameters.Add("@IsDoubleBed", filters.IsDoubleBed ?? null);
            }


            using var multi = await _db.QueryMultipleAsync("Rooms_GetAll", parameters, commandType: CommandType.StoredProcedure);

            var totalCount = await multi.ReadFirstOrDefaultAsync<int?>() ?? 0;
            var rooms = (await multi.ReadAsync<RoomResult>()).ToList();

            if (rooms.Any() && paginationModel.Depth >= 1)
            {
                var roomTypes = (await multi.ReadAsync<(int RoomTypeId, string RoomTypeName)>()).ToList();
                foreach (var room in rooms)
                {
                    var roomType = roomTypes.FirstOrDefault(rt => rt.RoomTypeId == room.RoomTypeId);
                    if (roomType != default)
                    {
                        room.RoomTypeName = roomType.RoomTypeName;
                    }
                }
            }

            return new PaginatedResult<RoomResult>(rooms, totalCount, paginationModel.PageNumber, paginationModel.PageSize);
        }


        public async Task<Room> UpdateRoom(int id, UpdateRoomDTO room)
        {


            try
            {

                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);

                if (room.RoomTypeId != null)
                    parameters.Add("@RoomTypeId", room.RoomTypeId);
                parameters.Add("@RoomNumber", room.RoomNumber);
                parameters.Add("@Floor", room.Floor);
                parameters.Add("@Status", room.Status?.ToString() ?? string.Empty);
                parameters.Add("@CleanStatus", room.CleanStatus?.ToString() ?? string.Empty);
                parameters.Add("@UpdatedAt", DateTime.UtcNow);

                var result = await _db.QueryFirstOrDefaultAsync<Room>(
                    "Rooms_Update", parameters, commandType: CommandType.StoredProcedure);

                return result;
            }
            catch (Exception ex)
            {

                throw new Exception("Error updating room: " + ex.Message);
            }
        }

    }
}