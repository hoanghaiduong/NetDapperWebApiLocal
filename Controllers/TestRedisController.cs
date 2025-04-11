using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetDapperWebApi.Common.Attributes;
using NetDapperWebApi.Common.Interfaces;
using NetDapperWebApi.Entities;

namespace NetDapperWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestRedisController : ControllerBase
    {
        private readonly IRedisCacheService _redis;
        private readonly IRoleService _roleService;

        public TestRedisController(IRoleService roleService, IRedisCacheService redis)
        {
            _roleService = roleService;
            _redis = redis;
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var dataCaching =  _redis.GetData<List<Role>>(nameof(Role));
            if (dataCaching is not null && dataCaching.Count != 0)
            {
                return Ok(new {
                    cache=dataCaching
                });
            }
            var roles = await _roleService.GetRoles();
            _redis.SetData(nameof(Role), roles.ToList());
            return Ok(new {
                database=roles
            });
        }
    }
}