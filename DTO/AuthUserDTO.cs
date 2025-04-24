using System.ComponentModel.DataAnnotations;

namespace morse_auth.DTO
{
    public class AuthUserDTO
    {
        [Required]
        public string? Login { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
