

using System.ComponentModel.DataAnnotations;

namespace NetDapperWebApi.Models
{
    public record RefreshTokenModel(
        [Required]
        string RefreshToken
    );
}