using Microsoft.EntityFrameworkCore;
using morse_auth.Database.Models;

namespace morse_auth.Database
{
    /// <summary> Morse database context </summary>
    public class MSDBContext : DbContext
    {
        public MSDBContext(DbContextOptions<MSDBContext> options) : base(options)
        {}

        public DbSet<UserModel> Users { get; set; }
    }
}
