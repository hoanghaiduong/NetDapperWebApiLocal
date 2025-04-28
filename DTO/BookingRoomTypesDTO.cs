using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NetDapperWebApi_local.DTO
{
    public class BookingRoomTypesDTO
    {
        [JsonIgnore]
        public int BookingId { get; set; }
        public int RoomTypeId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
    }
}