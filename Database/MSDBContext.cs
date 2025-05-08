using Microsoft.EntityFrameworkCore;
using morse_auth.Database.Models;

namespace morse_auth.Database
{
    /// <summary> Morse security database context </summary>
    public class MSDBContext : DbContext
    {
        public MSDBContext(DbContextOptions<MSDBContext> options) : base(options)
        {}

        public DbSet<UserModel> Users { get; set; }

        public DbSet<RefreshTokenModel> RefreshTokens { get; set; }
    }
}
