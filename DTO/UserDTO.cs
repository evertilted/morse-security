using System.ComponentModel.DataAnnotations;

namespace morse_auth.DTO
{
    public class UserDTO
    {
        public object? AccessToken { get; set; }
        public int? Id { get; set; }
        public string? Login { get; set; }
        public string? DisplayName { get; set; }
    }
}
