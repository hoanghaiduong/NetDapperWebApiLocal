using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using NetDapperWebApi_local.Common.Interfaces;
using NetDapperWebApi_local.DTO.Creates;
using NetDapperWebApi_local.Entities;

namespace NetDapperWebApi_local.Services
{
    public class ServiceUsageService : IServiceUsageService
    {
        private readonly IDbConnection _db;

        public ServiceUsageService(IDbConnection db)
        {
            _db = db;
        }

        public async Task<ServiceUsage> CreateServiceUsageAsync(CreateServiceUsageDTO serviceUsage)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@BookingId", serviceUsage.BookingId);
            parameters.Add("@ServiceId", serviceUsage.ServiceId);
            parameters.Add("@Quantity", serviceUsage.Quantity);
            // parameters.Add("@TotalPrice", serviceUsage.TotalPrice);
            parameters.Add("@UsedAt", serviceUsage.UsedAt);
            var result = await _db.QueryFirstOrDefaultAsync<ServiceUsage>("ServiceUsages_Create", parameters, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<Booking> GetAllByBookingIdAsync(int bookingId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@BookingId", bookingId);

            using var multi = await _db.QueryMultipleAsync(
                   "ServiceUsages_GetByBookingId",
                   parameters,
                   commandType: CommandType.StoredProcedure
               );
            var booking = await multi.ReadSingleOrDefaultAsync<Booking>();
            var services = (await multi.ReadAsync<Service>()).ToList();
            var servicesUsage = (await multi.ReadAsync<ServiceUsage>()).ToList();
            foreach (var item in services)
            {
                item.ServiceUsages = servicesUsage.Where(x => x.ServiceId == item.Id).ToList();
            }
            booking.Services = services;
            return booking;
        }
    }
}