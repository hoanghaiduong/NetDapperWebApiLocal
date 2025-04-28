using System.ComponentModel.DataAnnotations;

namespace NetDapperWebApi_local.DTO
{
    public record AuthDTO(
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        string Email,

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        string Password
    );
}
