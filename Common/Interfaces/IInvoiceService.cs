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
    public interface IInvoiceService
    {
        Task<Invoice> CreateInvoiceAsync(int bookingId);
        Task<Invoice> GetInvoiceByIdAsync(int id, int depth);
        Task<PaginatedResult<Invoice>> GetAllInvoicesAsync(PaginationModel dto);
        Task<Invoice> UpdateInvoiceAsync(int id, UpdateInvoiceDTO invoiceDto);
        Task<bool> DeleteInvoiceAsync(int id);
    }
}