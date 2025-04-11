
using NetDapperWebApi.DTO;
using NetDapperWebApi.DTO.Creates;
using NetDapperWebApi.DTO.Updates;
using NetDapperWebApi.Entities;
using NetDapperWebApi.Models;
using NetDapperWebApi.Services;

namespace NetDapperWebApi.Common.Interfaces
{
    public interface IHotelService
    {
        Task<string> AddAmenitiesToHotelAsync(AddRelationsMM<int, int> dto);
        Task<Hotel> CreateHotel(CreateHotelDTO hotel);
        Task<Hotel> GetHotel(int id, int depth);
        Task<Hotel> GetHotelByName(string name);
        Task<PaginatedResult<Hotel>> GetAllHotels(PaginationModel paginationModel);
        Task<Hotel> UpdateHotel(int id, UpdateHotelDTO hotel);
        Task<bool> DeleteHotel(int id);
    }
}