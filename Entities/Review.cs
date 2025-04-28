using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NetDapperWebApi_local.Entities
{
    public class Review : BaseEntity<int>
    {
        [Required]
        public int BookingId { get; set; }

        [Required]
        public int Rating { get; set; }

        [Required, MaxLength(500)]
        public string Comment { get; set; }

        // Navigation Properties
        [JsonIgnore]
        public virtual Booking Booking { get; set; }
    }
}