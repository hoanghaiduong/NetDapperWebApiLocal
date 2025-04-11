using NetDapperWebApi.DTO;
using NetDapperWebApi.DTO.Creates.Rooms;
using NetDapperWebApi.DTO.Updates;
using NetDapperWebApi.Entities;
using NetDapperWebApi.Models;

namespace NetDapperWebApi.Common.Interfaces
{
    public interface IRoomService
    {

        Task<Room> CreateRoom(CreateRoomDTO Room);
        Task<Room> GetRoom(int id, int depth);
        Task<PaginatedResult<Room>> GetRooms(PaginationModel dto);
        Task<Room> UpdateRoom(int id, UpdateRoomDTO Room);
        Task<bool> DeleteRoom(int id);
    }
}