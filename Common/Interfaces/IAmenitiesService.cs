
using NetDapperWebApi_local.DTO.Creates;
using NetDapperWebApi_local.DTO.Updates;
using NetDapperWebApi_local.Entities;
using NetDapperWebApi_local.Models;

namespace NetDapperWebApi_local.Common.Interfaces
{
    public interface IAmenitiesService
    {

        Task<PaginatedResult<Amenities>> GetAllAmenitiesAsync(PaginationModel pagination);
        Task<Amenities> GetAmenitiesByIdAsync(int id, int depth);
        Task<Amenities> CreateAmenitiesAsync(CreateAmenitiesDTO dto);
        Task<Amenities> UpdateAmenitiesAsync(int id, UpdateAmenitiesDTO dto);
        Task<bool> DeleteAmenitiesAsync(int AmenitiesId);
        Task<bool> DeleteAmenityIdsAsync(int[] amenityIds);

    }
}