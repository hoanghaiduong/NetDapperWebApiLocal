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
    public class PaymentService : IPaymentService
    {
        private readonly IDbConnection _db;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(IDbConnection db, ILogger<PaymentService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<Payment> CreatePaymentAsync(CreatePaymentDTO dto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@InvoiceId", dto.InvoiceId, DbType.Int32);
                parameters.Add("@Amount", dto.Amount, DbType.Decimal);
                parameters.Add("@PaymentMethod", dto.PaymentMethod, DbType.String);
                parameters.Add("@PaymentDate", dto.PaymentDate, DbType.DateTime);

                // Giả sử procedure tên là "Payments_Create"
                var payment = await _db.QueryFirstOrDefaultAsync<Payment>(
                    "Payments_Create", parameters, commandType: CommandType.StoredProcedure);
                return payment;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating payment.");
                throw;
            }
        }


        public async Task<Payment> GetPaymentByIdAsync(int id, int depth = 0)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id, DbType.Int32);
                parameters.Add("@Depth", depth, DbType.Int32);

                using var multi = await _db.QueryMultipleAsync("Payments_GetById", parameters, commandType: CommandType.StoredProcedure);
                var payment = await multi.ReadFirstOrDefaultAsync<Payment>();
                if (payment == null)
                    return null;

                if (depth >= 1)
                {
                    // Nếu muốn trả thêm thông tin của Invoice liên quan
                    var invoice = await multi.ReadFirstOrDefaultAsync<Invoice>();
                    payment.Invoice = invoice;
                }
                return payment;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting payment with id {id}");
                throw;
            }
        }

        public async Task<PaginatedResult<Payment>> GetAllPaymentsAsync(PaginationModel dto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PageNumber", dto.PageNumber, DbType.Int32);
                parameters.Add("@PageSize", dto.PageSize, DbType.Int32);
                parameters.Add("@Depth", dto.Depth ?? 0, DbType.Int32);
                parameters.Add("@Search", dto.Search ?? string.Empty, DbType.String);

                using var multi = await _db.QueryMultipleAsync("Payments_GetAll", parameters, commandType: CommandType.StoredProcedure);

                int totalCount = await multi.ReadSingleAsync<int>();
                var payments = (await multi.ReadAsync<Payment>()).ToList();

                if (dto.Depth >= 1)
                {
                    // Lấy thông tin Invoice liên quan với mỗi Payment
                    var invoices = (await multi.ReadAsync<Invoice>()).ToList();
                    foreach (var payment in payments)
                    {
                        payment.Invoice = invoices.FirstOrDefault(i => i.Id == payment.InvoiceId);
                    }
                }

                return new PaginatedResult<Payment>(payments, totalCount, dto.PageSize, dto.PageNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting paginated payments.");
                throw;
            }
        }
        public async Task<Payment> UpdatePaymentAsync(int id, UpdatePaymentDTO dto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id, DbType.Int32);
                // parameters.Add("@InvoiceId", dto.InvoiceId, DbType.Int32);
                parameters.Add("@Amount", dto.Amount, DbType.Decimal);
                parameters.Add("@PaymentMethod", dto.PaymentMethod, DbType.String);
                if (!string.IsNullOrEmpty(dto.Status))
                {
                    parameters.Add("@Status", dto.Status, DbType.String);
                }

                parameters.Add("@PaymentDate", dto.PaymentDate, DbType.DateTime);

                var payment = await _db.QueryFirstOrDefaultAsync<Payment>(
                    "Payments_Update", parameters, commandType: CommandType.StoredProcedure);
                return payment;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating payment with id {id}");
                throw;
            }
        }
        public async Task<bool> DeletePaymentAsync(int id)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id, DbType.Int32);
                var result = await _db.ExecuteAsync("Payments_Delete", parameters, commandType: CommandType.StoredProcedure);
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting payment with id {id}");
                throw;
            }
        }
    }
}