using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetDapperWebApi_local.Common.Interfaces;
using NetDapperWebApi_local.Entities;
using NetDapperWebApi_local.Models;

namespace NetDapperWebApi_local.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceTypesController : ControllerBase
    {
        private readonly IServiceTypeService _service;

        public ServiceTypesController(IServiceTypeService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ServiceTypeDTO dto)
        {
            try
            {
                var result = await _service.CreateServiceType(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>(false, message: "Thêm thất bại", errors: ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _service.GetServiceTypeById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>(false, message: "Không tìm thấy", errors: ex.Message));
            }
        }

        [HttpGet("name")]
        public async Task<IActionResult> GetByName([FromQuery] ServiceTypeDTO dto)
        {
            try
            {
                var result = await _service.GetServiceTypeByName(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>(false, message: "Không tìm thấy", errors: ex.Message));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationModel model)
        {

            var result = await _service.GetAllServiceType(model);
            return Ok(result);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] ServiceTypeDTO dto)
        {
            try
            {
                var result = await _service.UpdateServiceType(id, dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>(false, message: "Cập nhật thất bại", errors: ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _service.DeleteServiceTypeById(id);
                if (!success) return NotFound(new ApiResponse<object>(false, message: "Không tìm thấy"));
                return Ok(new ApiResponse<object>(true, message: "Xoá thành công"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>(false, message: "Xoá thất bại", errors: ex.Message));
            }
        }

        [HttpDelete("ids")]
        public async Task<IActionResult> DeleteServiceTypes([FromBody] int[] ids)
        {
            try
            {
                var result = await _service.DeleteServiceTypesAsync(ids);
                return Ok(new ApiResponse<object>(true, message: $"Xoá ServiceType với id = {string.Join(",", ids)} thành công !"));
            }
            catch (System.Exception ex)
            {
                return BadRequest(new ApiResponse<object>(false, message: "Xoá thất bại", errors: ex.Message));
            }
        }
    }
}