using Microsoft.EntityFrameworkCore;
using todo.Models;

namespace Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {

        }
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Misson> Missons => base.Set<Misson>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
