using Microsoft.EntityFrameworkCore;
using UrlRedirectApp.Data.Models;

namespace UrlRedirectApp.Data
{

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base()
        {
        }
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UrlMapping>().HasIndex(u => u.shortUrl).IsUnique();
        }
        public DbSet<UrlMapping> UrlMappings => Set<UrlMapping>();
    }
}
