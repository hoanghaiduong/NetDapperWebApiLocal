using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using NetDapperWebApi.Common.Enums;

namespace NetDapperWebApi.Entities
{
    public class Booking : BaseEntity<int>
    {
        public string? BookingCode { get; set; } = Guid.NewGuid().ToString("N").ToUpper(); // Mã đặt phòng
        public string? Notes { get; set; } // Ghi chú đặt phòng
        public int Adults { get; set; }
        public int Children { get; set; }
        public int? RoomCount { get; set; }//số phòng
        public string? ArrivalTime { get; set; } // Thời gian đến dự kiến
        public DateTime? CheckInDate { get; set; } = DateTime.Now;
        public DateTime? CheckOutDate { get; set; } = DateTime.Now;
        public EBookingStatus? Status { get; set; } = EBookingStatus.Pending;
        public decimal? BasePrice { get; set; } = 0;
        public decimal? TotalPrice { get; set; } = 0;
        public int ServiceTotalPrice
        {
            get
            {
                return Services?.Sum(s => (int)s.Price * s.ServiceUsages.Sum(su => su.Quantity)) ?? 0;
            }
        }

        public int? UserId { get; set; } // Id người đặt phòng
        // Navigation Properties
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public virtual User? User { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public virtual List<BookingRoomTypes> BookingRoomTypes { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public virtual List<Service>? Services { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public virtual List<Invoice>? Invoices { get; set; }
    
    }
}