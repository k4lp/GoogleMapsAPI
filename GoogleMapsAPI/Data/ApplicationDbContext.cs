using GoogleMapsAPI.Models.Entities;
using GoogleMapsAPI.Models.Entities.GoogleMaps;

using Microsoft.EntityFrameworkCore;

namespace GoogleMapsAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<User>? Users { get; set; }
        public DbSet<GoogleMapsRoot>? GoogleMapsRoots { get; set; }
        public DbSet<GoogleMapsResult>? GoogleMapsResults { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Different email for obvious reason
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
            // Different Username for obvious reason
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();
            // Making API hash column unique in order to not store same API and to enforce each user having its own API key.
            modelBuilder.Entity<User>()
                .HasIndex(u => u.ApiHash);
            base.OnModelCreating(modelBuilder);
        }
    }
}
