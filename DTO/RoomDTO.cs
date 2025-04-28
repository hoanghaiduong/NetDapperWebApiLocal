
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using NetDapperWebApi_local.Common.Enums;
using NetDapperWebApi_local.Entities;
using NetDapperWebApi_local.Common.Enums;

namespace NetDapperWebApi_local.DTO
{
    public class RoomDTO
    {

        [Required]
        public int RoomTypeId { get; set; }
        [Required]
        public string RoomNumber { get; set; }

        public int? Floor { get; set; }

        public ERoomStatus? Status { get; set; } = ERoomStatus.Empty;
        public ECleanStatus? CleanStatus { get; set; } = ECleanStatus.Ready;

    }

    public class RoomFilters
    {
        public ERoomStatus? Status { get; set; } 
        public ECleanStatus? CleanStatus { get; set; } 
        public bool? IsSingleBed { get; set; } // true = chỉ lấy phòng có SingleBed > 0
        public bool? IsDoubleBed { get; set; } // true = chỉ lấy phòng có DoubleBed > 0
    }
}