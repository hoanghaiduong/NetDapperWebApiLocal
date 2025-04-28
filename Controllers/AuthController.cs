using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDapperWebApi_local.Common.Interfaces;
using NetDapperWebApi_local.DTO;
using NetDapperWebApi_local.Entities;
using NetDapperWebApi_local.Models;

namespace NetDapperWebApi_local.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly IJwtService _jwtService;
        public AuthController(ILogger<AuthController> logger, IUserService userService, IAuthService authService, IJwtService jwtService)
        {
            _logger = logger;
            _userService = userService;
            _authService = authService;
            _jwtService = jwtService;
        }
        //test api authorzation

        // [HttpGet,Authorize]
        // public async Task<IResult> Test(){
        //     try
        //     {
        //         var uid = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //         var email=User.FindFirstValue(ClaimTypes.Email);
        //         return Results.Ok(new {
        //             message="Test",
        //             data=new {
        //                 email,
        //                 uid
        //             }
        //         });
        //     }
        //     catch (System.Exception)
        //     {

        //         throw;
        //     }
        // }

        [HttpPost("grant-roles"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GrantRoleAsync([FromBody] AddRelationsMM<int, int> dto)
        {
            try
            {
                var result = await _authService.GrantRolesToUser(dto);
                if (result == null)
                {
                    return BadRequest();
                }
                return Ok(new { result });
            }
            catch (System.Exception ex)
            {

                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] AuthDTO dto)
        {
            try
            {
                var result = await _authService.SignInAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }
        [HttpPost("signup")]
        public async Task<IResult> SignUp([FromBody] AuthDTO dto)
        {
            try
            {
                var result = await _authService.SignUpAsync(dto);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IResult> RefreshToken([FromBody] RefreshTokenModel dto)
        {
            try
            {
                var accessToken = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                // Trích xuất claims từ access token hết hạn
                var principal = _jwtService.GetPrincipalFromExpiredToken(accessToken);
                var uid = principal.FindFirstValue(ClaimTypes.NameIdentifier); // LẤY USER ID Ở ĐÂY!

                var result = await _authService.RefreshToken(dto, uid, accessToken);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

    }
}