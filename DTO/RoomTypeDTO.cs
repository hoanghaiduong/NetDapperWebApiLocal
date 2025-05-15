using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using NetDapperWebApi_local.Entities;

namespace NetDapperWebApi_local.DTO
{
    public class RoomTypeDTO
    {

        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal PricePerNight { get; set; }
        public decimal? PricePerHour { get; set; }
        public int NumberOfBathrooms { get; set; }
        public int NumberOfBeds { get; set; }
        public int SingleBed { get; set; }
        public int DoubleBed { get; set; }
        public int Capacity { get; set; }
        public int Sizes { get; set; }
        public IFormFile? Thumbnail { get; set; }
        public List<IFormFile>? Images { get; set; }
    }
}