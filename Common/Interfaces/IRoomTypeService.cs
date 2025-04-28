using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetDapperWebApi_local.DTO;
using NetDapperWebApi_local.DTO.Creates;
using NetDapperWebApi_local.DTO.Updates;
using NetDapperWebApi_local.Entities;
using NetDapperWebApi_local.Models;
using NetDapperWebApi_local.Services;

namespace NetDapperWebApi_local.Common.Interfaces
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