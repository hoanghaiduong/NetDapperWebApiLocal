
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using NetDapperWebApi.Common.Enums;
using NetDapperWebApi.Entities;

namespace NetDapperWebApi.DTO
{
    public class RoomDTO
    {
 
        [Required]
        public int RoomTypeId { get; set; }
        [Required]
        public string RoomNumber { get; set; }

        public int? Floor { get; set; }

        
        public ERoomStatus? Status { get; set; } = ERoomStatus.Ready;


        public IFormFile? Thumbnail { get; set; }

        public List<IFormFile>? Images { get; set; }



    }
}