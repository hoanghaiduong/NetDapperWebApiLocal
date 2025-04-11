using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NetDapperWebApi.Entities
{
    public class User : BaseEntity<int>
    {

        [Required, EmailAddress, MaxLength(255)]
        public string Email { get; set; } = null!;

        [Required, MaxLength(255)]
        [JsonIgnore]
        public string PasswordHash { get; set; } = null!;

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [MaxLength(50)]
        public string? FirstName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? LastName { get; set; } = string.Empty;

        public bool? EmailVerified { get; set; } = false;
        public string? Avatar { get; set; } = string.Empty;
        public string? RefreshToken { get; set; } = string.Empty;
        public DateTime? RefreshTokenExpiryDate { get; set; }
        public bool IsDisabled { get; set; } = false;
        public DateTime? LastLogin { get; set; }
        [JsonIgnore]
        public int? HotelId { get; set; } = null;


        // Navigation Properties
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public virtual Hotel? Hotel { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public virtual List<Role>? Roles { get; set; } = null;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public virtual List<UserRole>? UserRoles { get; set; } = null;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public virtual List<Booking>? Bookings { get; set; } = null;
    }
}