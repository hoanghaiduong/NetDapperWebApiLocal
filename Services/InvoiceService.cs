using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using NetDapperWebApi_local.Common.Interfaces;
using NetDapperWebApi_local.DTO.Creates;
using NetDapperWebApi_local.DTO.Updates;
using NetDapperWebApi_local.Entities;
using NetDapperWebApi_local.Models;

namespace NetDapperWebApi_local.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IDbConnection _db;
        private readonly ILogger<InvoiceService> _logger;

        public InvoiceService(IDbConnection db, ILogger<InvoiceService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<Invoice> CreateInvoiceAsync(int bookingId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@BookingId", bookingId);
                // parameters.Add("@TotalAmount", dto.TotalAmount);
                // parameters.Add("@PaidAmount", dto.PaidAmount);
                // parameters.Add("@DueAmount", dto.DueAmount);
                // Lưu ý: Trong cơ sở dữ liệu Status là NVARCHAR nên chuyển enum thành string

                // Giả sử stored procedure đặt tên là "Invoices_Create"
                var invoice = await _db.QueryFirstOrDefaultAsync<Invoice>(
                    "Invoices_Create", parameters, commandType: CommandType.StoredProcedure);
                return invoice;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating invoice.");
                throw;
            }
        }

        public async Task<bool> DeleteInvoiceAsync(int id)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id, DbType.Int32);

                // Giả sử stored procedure đặt tên là "Invoices_Delete"
                var result = await _db.ExecuteAsync(
                    "Invoices_Delete", parameters, commandType: CommandType.StoredProcedure);
                return result != -1;//xoá thành công ==-1
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public async Task<PaginatedResult<Invoice>> GetAllInvoicesAsync(PaginationModel dto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PageNumber", dto.PageNumber, DbType.Int32);
                parameters.Add("@PageSize", dto.PageSize, DbType.Int32);
                parameters.Add("@Depth", dto.Depth ?? 0, DbType.Int32);
                parameters.Add("@Search", dto.Search ?? string.Empty, DbType.String);

                using var multi = await _db.QueryMultipleAsync(
                    "Invoices_GetAll",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                // 1. Lấy tổng số bản ghi
                int totalCount = await multi.ReadSingleAsync<int>();

                // 2. Lấy danh sách Invoice theo phân trang
                var invoices = (await multi.ReadAsync<Invoice>()).ToList();

                // 3. Nếu Depth >= 1: lấy thông tin Payment liên quan đến Invoice
                if (dto.Depth >= 1)
                {
                    var bookings = (await multi.ReadAsync<Booking>()).ToList();

                    foreach (var invoice in invoices)
                    {
                        // Giả định Invoice có thuộc tính Booking kiểu Booking
                        invoice.Booking = bookings.FirstOrDefault(b => b.Id == invoice.BookingId);
                    }
                    // Stored procedure trả về Payment với cột InvoiceId (để liên kết)
                    var payments = (await multi.ReadAsync<Payment>()).ToList();

                    // Gán danh sách Payment cho từng Invoice
                    foreach (var invoice in invoices)
                    {
                        // Giả định Invoice có thuộc tính Payments kiểu List<Payment>
                        invoice.Payments = payments.Where(p => p.InvoiceId == invoice.Id).ToList();
                    }
                }


                return new PaginatedResult<Invoice>(invoices, totalCount, dto.PageSize, dto.PageNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting paginated invoices.");
                throw;
            }
        }

        public async Task<Invoice> GetInvoiceByIdAsync(int id, int depth)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            parameters.Add("@Depth", depth);

            // Giả sử stored procedure đặt tên là "Invoices_GetById"
            using var multi = await _db.QueryMultipleAsync(
                "Invoices_GetById", parameters, commandType: CommandType.StoredProcedure);

            var invoice = await multi.ReadFirstOrDefaultAsync<Invoice>();
            if (depth >= 1)
            {
                //lấy ra booking thuộc invoice đó
                var booking = await multi.ReadSingleOrDefaultAsync<Booking>();
                invoice.Booking = booking;
                //lấy ra danh sách payment thuộc hoá đơn đó
                var payments = (await multi.ReadAsync<Payment>()).ToList();
                invoice.Payments = payments;
            }
            return invoice;
        }

        public async Task<Invoice> UpdateInvoiceAsync(int id, UpdateInvoiceDTO invoiceDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id, DbType.Int32);
                parameters.Add("@BookingId", invoiceDto.BookingId, DbType.Int32);
                parameters.Add("@TotalAmount", invoiceDto.TotalAmount, DbType.Decimal);
                parameters.Add("@PaidAmount", invoiceDto.PaidAmount, DbType.Decimal);
                parameters.Add("@DueAmount", invoiceDto.DueAmount, DbType.Decimal);
                parameters.Add("@Status", invoiceDto.Status.ToString(), DbType.String);

                // Giả sử stored procedure đặt tên là "Invoices_Update"
                var invoice = await _db.QueryFirstOrDefaultAsync<Invoice>(
                    "Invoices_Update", parameters, commandType: CommandType.StoredProcedure);
                return invoice;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating invoice with id {id}");
                throw;
            }
        }
    }
}