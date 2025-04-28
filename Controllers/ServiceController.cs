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
    public class ServiceController : ControllerBase
    {

        private readonly IServiceServices _serviceServices;

        public ServiceController(IServiceServices serviceServices)
        {
            _serviceServices = serviceServices;
        }

        //crud  
        [HttpPost]
        public async Task<IActionResult> CreateService([FromBody] CreateServiceDTO dto)
        {
            try
            {
                var result = await _serviceServices.CreateService(dto);
                return Ok(new { result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        //Get By Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceById(int id, [FromQuery] int depth = 0)
        {
            try
            {
                var result = await _serviceServices.GetServiceById(id, depth);
                return Ok(new { result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        //Get All
        [HttpGet]
        public async Task<IActionResult> GetAllServices([FromQuery] PaginationModel dto)
        {
            try
            {
                var result = await _serviceServices.GetServices(dto);
                return Ok(new { result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        //update
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateService(int id, [FromBody] UpdateServiceDTO dto)
        {
            try
            {
                var result = await _serviceServices.UpdateService(id, dto);
                return Ok(new { result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        //DELETE 
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            try
            {
                var result = await _serviceServices.DeleteService(id);
                return Ok(new { result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}