

using System.ComponentModel.DataAnnotations;

namespace NetDapperWebApi_local.Models
{
    public record RefreshTokenModel(
        [Required]
        string RefreshToken
    );
}