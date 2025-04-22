using System.ComponentModel.DataAnnotations;

namespace morse_auth.DTO
{
    public class UserDTO
    {
        public int? Id { get; set; }
        [Required]
        public string? Login { get; set; }
        [Required]
        public string? Password { get; set; }
        public string? DisplayName { get; set; }
    }
}
