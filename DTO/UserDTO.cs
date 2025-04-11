
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NetDapperWebApi.DTO
{
    public class UserDTO
    {

        public string? PhoneNumber { get; set; }
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        [DefaultValue(false)]
        public bool? EmailVerified { get; set; } = false;
        public IFormFile? Avatar { get; set; } = null;
        public string? RefreshToken { get; set; } = string.Empty;
        public DateTime? RefreshTokenExpiryDate { get; set; }
        [DefaultValue(false)]
        public bool IsDisabled { get; set; } = false;
        public DateTime? LastLogin { get; set; }
        public int? HotelId { get; set; }
        public List<int>? Roles { get; set; } = [];
    }
}