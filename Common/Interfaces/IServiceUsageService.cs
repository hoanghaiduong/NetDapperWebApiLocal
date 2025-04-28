using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetDapperWebApi_local.DTO.Creates;
using NetDapperWebApi_local.Entities;

namespace NetDapperWebApi_local.Common.Interfaces
{
    public interface IServiceUsageService
    {
        Task<ServiceUsage> CreateServiceUsageAsync(CreateServiceUsageDTO serviceUsage);
        Task<Booking> GetAllByBookingIdAsync(int bookingId);
    }
}