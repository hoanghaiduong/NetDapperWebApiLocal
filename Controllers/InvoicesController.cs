using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetDapperWebApi_local.Common.Interfaces;
using NetDapperWebApi_local.DTO.Creates;
using NetDapperWebApi_local.DTO.Updates;
using NetDapperWebApi_local.Models;

namespace NetDapperWebApi_local.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoicesController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateInvoice([FromQuery] int bookingId)
        {
            var createdInvoice = await _invoiceService.CreateInvoiceAsync(bookingId);
            return Ok(new { createdInvoice });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInvoiceById(int id, [FromQuery] int depth = 0)
        {
            var invoice = await _invoiceService.GetInvoiceByIdAsync(id, depth);
            if (invoice == null)
            {
                return NotFound();
            }
            return Ok(invoice);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllInvoices([FromQuery] PaginationModel dto)
        {
            var invoices = await _invoiceService.GetAllInvoicesAsync(dto);
            return Ok(invoices);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInvoice(int id, [FromBody] UpdateInvoiceDTO invoiceDto)
        {
            if (invoiceDto == null)
            {
                return BadRequest("Invalid invoice data.");
            }

            var updatedInvoice = await _invoiceService.UpdateInvoiceAsync(id, invoiceDto);
            if (updatedInvoice == null)
            {
                return NotFound();
            }
            return Ok(updatedInvoice);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            var deleted = await _invoiceService.DeleteInvoiceAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}