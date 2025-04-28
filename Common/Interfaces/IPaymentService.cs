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
    public interface IPaymentService
    {
        Task<Payment> CreatePaymentAsync(CreatePaymentDTO dto);
        Task<Payment> GetPaymentByIdAsync(int id, int depth = 0);
        Task<PaginatedResult<Payment>> GetAllPaymentsAsync(PaginationModel dto);
        Task<Payment> UpdatePaymentAsync(int id, UpdatePaymentDTO dto);
        Task<bool> DeletePaymentAsync(int id);
    }
}