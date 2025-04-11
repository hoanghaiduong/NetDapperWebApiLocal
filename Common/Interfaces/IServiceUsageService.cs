using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetDapperWebApi.DTO.Creates;
using NetDapperWebApi.Entities;

namespace NetDapperWebApi.Common.Interfaces
{
    public interface IServiceUsageService
    {
        Task<ServiceUsage> CreateServiceUsageAsync(CreateServiceUsageDTO serviceUsage);
        Task<Booking> GetAllByBookingIdAsync(int bookingId);
    }
}