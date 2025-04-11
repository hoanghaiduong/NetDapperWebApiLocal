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
    public class AmenitieController : ControllerBase
    {
        private readonly IAmenitiesService _amenitieService;

        public AmenitieController(IAmenitiesService AmenitieService)
        {
            _amenitieService = AmenitieService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAmenities([FromQuery] PaginationModel dto)
        {
            var amenities = await _amenitieService.GetAllAmenitiesAsync(dto);
            return Ok(amenities);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAmenitieById([FromRoute] int id, [FromQuery] int depth = 0)
        {
            var amenitie = await _amenitieService.GetAmenitiesByIdAsync(id, depth);
            if (amenitie == null) return NotFound();
            return Ok(amenitie);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAmenitie([FromBody] CreateAmenitiesDTO dto)
        {
            var result = await _amenitieService.CreateAmenitiesAsync(dto);
            return Ok(new { result });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAmenitie([FromRoute] int id, [FromBody] UpdateAmenitiesDTO dto)
        {

            var result = await _amenitieService.UpdateAmenitiesAsync(id, dto);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAmenitie(int id)
        {
            await _amenitieService.DeleteAmenitiesAsync(id);
            return Ok(new
            {
                message = $"Xoá amenitie với id = {id} thành công !"
            });
        }


    }
}