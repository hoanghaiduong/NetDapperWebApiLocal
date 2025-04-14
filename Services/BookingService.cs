using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using NetDapperWebApi.Common.Enums;
using NetDapperWebApi.Common.Interfaces;
using NetDapperWebApi.DTO.Creates;
using NetDapperWebApi.DTO.Updates;
using NetDapperWebApi.Entities;
using NetDapperWebApi.Models;

namespace NetDapperWebApi.Services
{
    public class BookingService : IBookingService
    {
        private readonly ILogger<BookingService> _logger;
        private readonly IDbConnection _db;

        public BookingService(ILogger<BookingService> logger, IDbConnection db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task<Booking> CreateAsync(CreateBookingDTO dto)
        {
            try
            {
                // Tạo DataTable cho TVP
                var dt = new DataTable();
                dt.Columns.Add("RoomTypeId", typeof(int));
                dt.Columns.Add("FullName", typeof(string));
                dt.Columns.Add("Email", typeof(string));

                foreach (var item in dto.BookingRoomTypes)
                {
                    dt.Rows.Add(item.RoomTypeId, item.FullName, item.Email);
                    // _logger.LogInformation($"RoomTypeId: {item.RoomTypeId}, FullName: {item.FullName}, Email: {item.Email}");
                }
                var parameters = new DynamicParameters();
                parameters.Add("@Notes", dto.Notes);
                parameters.Add("@Adults", dto.Adults);
                parameters.Add("@Children", dto.Children);
                parameters.Add("@RoomCount", dt.Rows.Count);
                parameters.Add("@ArrivalTime", dto.ArrivalTime);
                parameters.Add("@CheckInDate", dto.CheckInDate);
                parameters.Add("@CheckOutDate", dto.CheckOutDate);
                if (dto.BasePrice > 0)
                {
                    parameters.Add("@BasePrice", dto.BasePrice);
                }

                parameters.Add("@UserId", dto.UserId);
                parameters.Add("@BookingRoomTypes", dt.AsTableValuedParameter("dbo.BookingRoomTypesTVP"));

                var result = await _db.QueryFirstOrDefaultAsync<Booking>(
                    "Booking_Create",
                    parameters,
                    commandType: CommandType.StoredProcedure);


                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception("Lỗi khi tạo booking");
            }


        }

        public async Task<bool> DeleteAsync(int id)
        {

            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);

                var result = await _db.ExecuteScalarAsync<int>(
                      "Booking_Delete", parameters,
                      commandType: CommandType.StoredProcedure
                      );


                return result != -1;
            }
            catch (Exception ex)
            {

                throw new Exception("Lỗi khi xóa booking: " + ex.Message, ex);
            }
        }

        public async Task<PaginatedResult<Booking>> GetAllAsync(PaginationModel dto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PageNumber", dto.PageNumber);
                parameters.Add("@PageSize", dto.PageSize);
                parameters.Add("@Depth", dto.Depth);
                parameters.Add("@Search", dto.Search);

                using var multi = await _db.QueryMultipleAsync(
                    "Bookings_GetAll",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                // Result set 1: TotalCount
                var totalCount = await multi.ReadFirstAsync<int>();
                // Result set 2: List of Bookings
                var bookings = (await multi.ReadAsync<Booking>()).AsList();
                List<BookingRoomTypes> bookingRoomTypes = null;
                List<RoomType> roomTypes = null;

                if (dto.Depth >= 1)
                    bookingRoomTypes = (await multi.ReadAsync<BookingRoomTypes>()).AsList();
                if (dto.Depth >= 2)
                    roomTypes = (await multi.ReadAsync<RoomType>()).AsList();

                return new PaginatedResult<Booking>(bookings, totalCount, dto.PageNumber, dto.PageSize);
            }

            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Booking> GetByIdAsync(int id, int depth)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            parameters.Add("@Depth", depth);
            using var multi = await _db.QueryMultipleAsync(
                "Bookings_GetByID",
                parameters,
                commandType: CommandType.StoredProcedure
            );
            var booking = await multi.ReadFirstOrDefaultAsync<Booking>();
            if (booking == null) return null!;
            if (depth >= 1)
            {
                var bookingRoomTypes = (await multi.ReadAsync<BookingRoomTypes>()).ToList();

                var roomTypes = (await multi.ReadAsync<RoomType>())
                    .GroupBy(rt => rt.Id) // Nhóm theo RoomTypeId
                    .Select(g => g.First()) // Lấy giá trị đầu tiên của nhóm
                    .ToDictionary(rt => rt.Id);

                foreach (var brt in bookingRoomTypes)
                {
                    if (roomTypes.TryGetValue(brt.RoomTypeId, out var roomType))
                    {
                        brt.RoomType = roomType;
                    }
                }
                booking.BookingRoomTypes = bookingRoomTypes;

                var services = (await multi.ReadAsync<Service>()).ToList();
                booking.Services = services;
                var invoices = (await multi.ReadAsync<Invoice>()).ToList();
                booking.Invoices = invoices;

                if (booking.UserId != null)
                {
                    var user = await multi.ReadFirstOrDefaultAsync<User>();
                    if (user != null)
                    {
                        booking.User = user;
                    }
                }


            }
         

            return booking;
        }

        public async Task<Booking> UpdateAsync(int id, UpdateBookingDTO dto)
        {

            try
            {
                // Tạo DataTable cho TVP
                var dt = new DataTable();
                dt.Columns.Add("RoomTypeId", typeof(int));
                dt.Columns.Add("FullName", typeof(string));
                dt.Columns.Add("Email", typeof(string));
                if (dto.BookingRoomTypes != null)
                {
                    foreach (var item in dto.BookingRoomTypes)
                    {
                        dt.Rows.Add(item.RoomTypeId, item.FullName, item.Email);
                    }
                }


                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);
                parameters.Add("@Notes", dto.Notes);
                parameters.Add("@Adults", dto.Adults);
                parameters.Add("@Children", dto.Children);
                parameters.Add("@RoomCount", dt.Rows.Count);
                parameters.Add("@ArrivalTime", dto.ArrivalTime);
                parameters.Add("@CheckInDate", dto.CheckInDate);
                parameters.Add("@CheckOutDate", dto.CheckOutDate);
                parameters.Add("@Status", dto.Status,DbType.String);
                parameters.Add("@BasePrice", dto.BasePrice);
                parameters.Add("@UserId", dto.UserId);
                parameters.Add("@BookingRoomTypes", dt.AsTableValuedParameter("dbo.BookingRoomTypesTVP"));

                var result = await _db.QueryFirstOrDefaultAsync<Booking>(
                    "Booking_Update",
                    parameters,
                    commandType: CommandType.StoredProcedure);


                return result;
            }
            catch (Exception ex)
            {

                throw new Exception("Lỗi khi cập nhật booking: " + ex.Message, ex);
            }
        }
    }
}