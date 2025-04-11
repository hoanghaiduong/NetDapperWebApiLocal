
using NetDapperWebApi.DTO.Creates;
using NetDapperWebApi.DTO.Updates;
using NetDapperWebApi.Entities;
using NetDapperWebApi.Models;

namespace NetDapperWebApi.Common.Interfaces
{
    public interface IAmenitiesService
    {

        Task<PaginatedResult<Amenities>> GetAllAmenitiesAsync(PaginationModel pagination);
        Task<Amenities> GetAmenitiesByIdAsync(int id, int depth);
        Task<Amenities> CreateAmenitiesAsync(CreateAmenitiesDTO dto);
        Task<Amenities> UpdateAmenitiesAsync(int id, UpdateAmenitiesDTO dto);
        Task<bool> DeleteAmenitiesAsync(int AmenitiesId);

    }
}