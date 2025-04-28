using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetDapperWebApi_local.DTO.Creates;
using NetDapperWebApi_local.DTO.Updates;
using NetDapperWebApi_local.Entities;
using NetDapperWebApi_local.Models;

namespace NetDapperWebApi_local.Common.Interfaces
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