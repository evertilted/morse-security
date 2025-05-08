using System.ComponentModel.DataAnnotations.Schema;

namespace morse_auth.Database.Models
{
    [Table("refresh_tokens")]
    public class RefreshTokenModel
    {
        [Column("user_id")]
        int UserId { get; set; }

        [Column("token")]
        string Token { get; set; }
    }
}
