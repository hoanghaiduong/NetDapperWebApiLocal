
using NetDapperWebApi.Entities;

namespace NetDapperWebApi.Models
{
    public class AuthResponse
    {
        public User User { get; set; } = null!;
        public TokenModel Token { get; set; } = null!;
    }
}