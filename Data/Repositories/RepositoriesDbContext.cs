using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace githubtriggerbot.Data.Repositories
{
    public class RepositoriesDbContext : DbContext
    {
        public RepositoriesDbContext(DbContextOptions<RepositoriesDbContext> options)
            : base(options)
        { }

        public DbSet<Repository> Repositories { get; set; }
        public DbSet<Hook> Hooks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Repository>().ToTable("repository");
            modelBuilder.Entity<Hook>().ToTable("hook");
        }
    }
}
