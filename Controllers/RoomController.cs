
using Microsoft.AspNetCore.Mvc;
using NetDapperWebApi.Common.Interfaces;
using NetDapperWebApi.DTO;
using NetDapperWebApi.DTO.Creates.Rooms;
using NetDapperWebApi.DTO.Updates;
using NetDapperWebApi.Entities;
using NetDapperWebApi.Models;


namespace NetDapperWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;
        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpPost]
        public async Task<IResult> Create([FromForm] CreateRoomDTO dto)
        {
            try
            {
                var result = await _roomService.CreateRoom(dto);
                return Results.Ok(new { result });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new
                {
                    ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IResult> GetById([FromRoute] int id, [FromQuery] int depth = 0)
        {
            var room = await _roomService.GetRoom(id, depth);
            if (room == null) return Results.NotFound();
            return Results.Ok(room);
        }

        [HttpGet]
        public async Task<IResult> GetAll([FromQuery] PaginationModel paginationModel)
        {
            var rooms = await _roomService.GetRooms(paginationModel);
            return Results.Ok(rooms);
        }

        [HttpPut("{id}")]
        public async Task<IResult> Update(int id, [FromForm] UpdateRoomDTO room)
        {
            var result = await _roomService.UpdateRoom(id, room);
            return Results.Ok(new { message = result });
        }

        [HttpDelete("{id}")]
        public async Task<IResult> Delete(int id)
        {
            var result = await _roomService.DeleteRoom(id);
            return Results.Ok(new { message = result });
        }

    
    }
}