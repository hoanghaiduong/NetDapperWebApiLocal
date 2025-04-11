using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetDapperWebApi.DTO;
using NetDapperWebApi.DTO.Creates;
using NetDapperWebApi.DTO.Updates;
using NetDapperWebApi.Entities;
using NetDapperWebApi.Models;
using NetDapperWebApi.Services;

namespace NetDapperWebApi.Common.Interfaces
{
    public interface IRoomTypeService
    {
        Task<string> AddAmenitiesToRoomTypeAsync(AddRelationsMM<int, int> dto);
        Task<RoomType> CreateRoomType(CreateRoomTypeDTO roomType);
        Task<RoomType> GetRoomType(int id,int depth);
        Task<PaginatedResult<RoomType>> GetRoomTypes(PaginationModel pagination);
        Task<RoomType> UpdateRoomType(int id, UpdateRoomTypeDTO roomType);
        Task<bool> DeleteRoomType(int id);
        // Task<RoomTypeDTOWithRoom> GetRoomTypeWithRooms(int id);
        // Task<RoomType> GetRoomTypeWithRooms(int id, int depth);
    }
}