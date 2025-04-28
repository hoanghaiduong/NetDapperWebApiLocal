
using NetDapperWebApi_local.Entities;

namespace NetDapperWebApi_local.Models
{
    public class AuthResponse
    {
        public User User { get; set; } = null!;
        public TokenModel Token { get; set; } = null!;
    }
}