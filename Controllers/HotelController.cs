using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetDapperWebApi.Common.Interfaces;
using NetDapperWebApi.DTO;
using NetDapperWebApi.DTO.Creates;
using NetDapperWebApi.DTO.Updates;
using NetDapperWebApi.Entities;
using NetDapperWebApi.Models;

namespace NetDapperWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelController : ControllerBase
    {
        private readonly IHotelService _hotelService;
        private readonly ILogger<HotelController> _logger;

        public HotelController(IHotelService hotelService, ILogger<HotelController> logger)
        {
            _hotelService = hotelService;
            _logger = logger;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateHotelDTO hotel)
        {
            try
            {
                var result = await _hotelService.CreateHotel(hotel);
                return Ok(new { message = result });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
        // Endpoint để thêm các Amenitie vào Hotel
        [HttpPost("add-amenities")]
        public async Task<IActionResult> AddAmenitiesToHotelAsync([FromBody] AddRelationsMM<int, int> dto)
        {
            try
            {
                var result = await _hotelService.AddAmenitiesToHotelAsync(dto);
                return Ok(new { result });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, [FromQuery] int depth = 0)
        {
            try
            {
                var hotel = await _hotelService.GetHotel(id, depth);
                if (hotel == null) return NotFound();
                return Ok(hotel);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("query")]
        public async Task<IActionResult> GetByName([FromQuery] string name)
        {
            try
            {
                var hotel = await _hotelService.GetHotelByName(name);
                if (hotel == null) return NotFound();
                return Ok(hotel);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationModel paginationModel)
        {
            try
            {
                var hotels = await _hotelService.GetAllHotels(paginationModel);
                return Ok(new
                {
                    data = hotels
                });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromForm] UpdateHotelDTO hotel)
        {

            var result = await _hotelService.UpdateHotel(id, hotel);
            return Ok(new { message = result });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _hotelService.DeleteHotel(id);
                return result ? Ok(new { message = $"Xoá Hotel với id={id} thành công" }) : BadRequest();
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }
}