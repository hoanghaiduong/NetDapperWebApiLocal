using NetDapperWebApi_local.DTO.Creates.Rooms;
using NetDapperWebApi_local.Entities;
using NetDapperWebApi_local.Models;
using NetDapperWebApi_local.DTO.Res;
using NetDapperWebApi_local.DTO.Updates;
using NetDapperWebApi_local.DTO;

namespace NetDapperWebApi_local.Common.Interfaces
{
    public interface IRoomService
    {

        Task<Room> CreateRoom(CreateRoomDTO Room);
        Task<Room> GetRoom(int id, int depth);
        Task<PaginatedResult<RoomResult>> GetRooms(PaginationModel dto,RoomFilters filters);
        Task<Room> UpdateRoom(int id, UpdateRoomDTO Room);
        Task<bool> DeleteRoom(int id);
    }
}