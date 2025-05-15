using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NetDapperWebApi_local.Entities
{
    public class BookingRoomTypes
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public int RoomTypeId { get; set; }
        public int RoomId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        [JsonIgnore]
        public virtual Booking? Booking { get; set; }
        [JsonIgnore]
        public virtual RoomType? RoomType { get; set; }

    }
}