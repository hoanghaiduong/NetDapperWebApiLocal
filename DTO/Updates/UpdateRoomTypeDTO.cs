using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetDapperWebApi.DTO.Updates
{
    public class UpdateRoomTypeDTO : RoomTypeDTO
    {
       
        public List<string>? KeptImages { get; set; }
    }
}