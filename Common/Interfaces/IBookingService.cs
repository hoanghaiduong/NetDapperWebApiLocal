using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetDapperWebApi.DTO.Creates;
using NetDapperWebApi.DTO.Updates;
using NetDapperWebApi.Entities;
using NetDapperWebApi.Models;

namespace NetDapperWebApi.Common.Interfaces
{
    public interface IBookingService
    {
        Task<PaginatedResult<Booking>> GetAllAsync(PaginationModel dto);
        Task<Booking> GetByIdAsync(int id, int depth);
        // Task<Booking> CreateAsync(CreateBookingDTO booking);
        Task<Booking> CreateAsync(CreateBookingDTO booking);
        Task<Booking> UpdateAsync(int id, UpdateBookingDTO booking);
        Task<bool> DeleteAsync(int id);
    }
}