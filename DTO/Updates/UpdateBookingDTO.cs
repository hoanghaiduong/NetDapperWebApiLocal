using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetDapperWebApi.DTO.Updates
{
    public class UpdateBookingDTO : BookingDTO
    {
        // public int Id { get; set; }
        public List<UpdateBookingRoomTypesDTO?>? BookingRoomTypes { get; set; }
    }
}