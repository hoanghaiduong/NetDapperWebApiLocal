using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetDapperWebApi.Common.Enums;
using NetDapperWebApi.Common.Interfaces;
using NetDapperWebApi.DTO.Creates;
using NetDapperWebApi.DTO.Updates;
using NetDapperWebApi.Entities;
using NetDapperWebApi.Models;

namespace NetDapperWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        // GET api/booking
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationModel dto)
        {
            var result = await _bookingService.GetAllAsync(dto);
            return Ok(result);
        }

        // GET api/booking/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id, [FromQuery] int depth = 0)
        {
            var booking = await _bookingService.GetByIdAsync(id, depth);
            if (booking == null)
                return NotFound();
            return Ok(booking);
        }

        // // POST api/booking
        // [HttpPost]
        // public async Task<IActionResult> Create([FromBody] CreateBookingDTO dto)
        // {
        //     if (!ModelState.IsValid)
        //         return BadRequest(ModelState);

        //     var booking = await _bookingService.CreateAsync(dto);
        //     return Ok(new {booking});
        // }

        // POST api/booking
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookingDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
         
            var booking = await _bookingService.CreateAsync(dto);
            return Ok(new { booking });
        }

        // PUT api/booking/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBookingDTO dto,[FromQuery] EBookingStatus? status=null)
        {
             dto.Status=status.ToString();
            var rowsAffected = await _bookingService.UpdateAsync(id, dto);
           
            return Ok(rowsAffected);
        }

        // DELETE api/booking/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var rowsAffected = await _bookingService.DeleteAsync(id);
            if (!rowsAffected)
                return NotFound();

            return Ok();
        }
    }
}