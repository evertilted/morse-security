using System.ComponentModel.DataAnnotations;

namespace morse_auth.DTO
{
    /// <summary>
    /// The data provided by the client on authentication
    /// </summary>
    public class AuthUserDTO
    {
        [Required]
        public string? Login { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
