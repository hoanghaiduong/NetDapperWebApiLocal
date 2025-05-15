using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NetDapperWebApi_local.DTO
{
    public class BookingRoomTypesDTO
    {
      
        public int RoomTypeId { get; set; }
        public int RoomId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
    }
}