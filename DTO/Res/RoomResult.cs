using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetDapperWebApi_local.DTO;
using NetDapperWebApi_local.Entities;

namespace NetDapperWebApi_local.DTO.Res
{
    public class RoomResult : RoomDTO
    {
        public int Id { get; set; }
        public int RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }
        public decimal PricePerNight { get; set; }
        public decimal? PricePerHour { get; set; }
    }
}