

using System.ComponentModel.DataAnnotations;

namespace NetDapperWebApi_local.DTO.Creates
{
    public class CreateUserDTO : UserDTO
    {
        [EmailAddress]
        public string Email { get; set; } = null!;

        public string? Password { get; set; }

    
    }
}