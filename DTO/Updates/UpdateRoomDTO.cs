using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using NetDapperWebApi_local.Common.Enums;
using NetDapperWebApi_local.Common.Enums;

namespace NetDapperWebApi_local.DTO.Updates
{
    public class UpdateRoomDTO
    {

        public int? RoomTypeId { get; set; }

        public string? RoomNumber { get; set; }

        public int? Floor { get; set; }


        public ERoomStatus? Status { get; set; } = ERoomStatus.Empty;
        public ECleanStatus? CleanStatus { get; set; } = ECleanStatus.Ready;

    }
}
