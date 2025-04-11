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
    public interface IPaymentService
    {
        Task<Payment> CreatePaymentAsync(CreatePaymentDTO dto);
        Task<Payment> GetPaymentByIdAsync(int id, int depth = 0);
        Task<PaginatedResult<Payment>> GetAllPaymentsAsync(PaginationModel dto);
        Task<Payment> UpdatePaymentAsync(int id, UpdatePaymentDTO dto);
        Task<bool> DeletePaymentAsync(int id);
    }
}