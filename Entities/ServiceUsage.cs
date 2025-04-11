using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NetDapperWebApi.Entities
{
    public class ServiceUsage : BaseEntity<int>
    {

        [Required]
        public int BookingId { get; set; }

        [Required]
        public int ServiceId { get; set; }

        public int Quantity { get; set; } = 1;
        public decimal TotalPrice { get; set; }
        public DateTime UsedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]


        public virtual Booking Booking { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        // Navigation Properties
        public virtual Service Service { get; set; }
    }
}