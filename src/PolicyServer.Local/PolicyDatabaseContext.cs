using Microsoft.EntityFrameworkCore;
using PolicyServer.Local;

namespace PolicyServer.Runtime.Client
{
    public class PolicyDatabaseContext : DbContext
    {
        public PolicyDatabaseContext(DbContextOptions<PolicyDatabaseContext> contextOptions) : base(
            contextOptions)
        {
        }
        
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}