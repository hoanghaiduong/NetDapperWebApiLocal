using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetDapperWebApi.Common.Enums;
using NetDapperWebApi.Common.Interfaces;
using NetDapperWebApi.DTO.Creates;
using NetDapperWebApi.DTO.Updates;
using NetDapperWebApi.Models;

namespace NetDapperWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentDTO dto, [FromQuery] EPaymentMethod paymentMethod = EPaymentMethod.Cash)
        {
            if (dto == null)
                return BadRequest("Invalid payment data.");
            dto.PaymentMethod = paymentMethod.ToString();
            var payment = await _paymentService.CreatePaymentAsync(dto);
            // return CreatedAtAction(nameof(GetPaymentById), new { id = payment.Id }, payment);
            return Ok(new { payment });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentById(int id, [FromQuery] int depth = 0)
        {
            var payment = await _paymentService.GetPaymentByIdAsync(id, depth);
            if (payment == null)
                return NotFound();
            return Ok(payment);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPayments([FromQuery] PaginationModel pagination)
        {
            var paginatedPayments = await _paymentService.GetAllPaymentsAsync(pagination);
            return Ok(paginatedPayments);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayment(int id, [FromBody] UpdatePaymentDTO dto, [FromQuery] EPaymentMethod? paymentMethod=null, [FromQuery] EPaymentStatus? paymentStatus=null)
        {
            if (dto == null)
                return BadRequest("Invalid payment data.");

            dto.PaymentMethod = paymentMethod.ToString();
            dto.Status = paymentStatus.ToString();

            var payment = await _paymentService.UpdatePaymentAsync(id, dto);
            if (payment == null)
                return NotFound();
            return Ok(new { payment });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var result = await _paymentService.DeletePaymentAsync(id);
            if (!result)
                return NotFound();
            return Ok();
        }
    }
}