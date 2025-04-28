using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using NetDapperWebApi_local.Common.Enums;

namespace NetDapperWebApi_local.Entities
{
    public class Room : BaseEntity<int>
    {
        [JsonIgnore]
        public int RoomTypeId { get; set; }
        public string RoomNumber { get; set; }
        public int? Floor { get; set; }
        public ERoomStatus? Status { get; set; }
        public ECleanStatus? CleanStatus { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public virtual RoomType? RoomType { get; set; } = null;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public virtual ICollection<Booking> Bookings { get; set; } = [];

    }



}