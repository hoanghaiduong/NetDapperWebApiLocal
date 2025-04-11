

using System.ComponentModel.DataAnnotations;

namespace NetDapperWebApi.DTO.Creates
{
    public class CreateUserDTO : UserDTO
    {
        [EmailAddress]
        public string Email { get; set; } = null!;

        public string? Password { get; set; }

    
    }
}