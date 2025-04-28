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
    public interface IInvoiceService
    {
        Task<Invoice> CreateInvoiceAsync(int bookingId);
        Task<Invoice> GetInvoiceByIdAsync(int id, int depth);
        Task<PaginatedResult<Invoice>> GetAllInvoicesAsync(PaginationModel dto);
        Task<Invoice> UpdateInvoiceAsync(int id, UpdateInvoiceDTO invoiceDto);
        Task<bool> DeleteInvoiceAsync(int id);
    }
}