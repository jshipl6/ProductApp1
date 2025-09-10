using Microsoft.EntityFrameworkCore;
using ProductApp.Models;

namespace ProductApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Product> Products => Set<Product>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed a few rows so the table isn't empty at first run
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Guitar Picks", Price = 4.99m },
                new Product { Id = 2, Name = "Patch", Price = 6.50m },
                new Product { Id = 3, Name = "Sticker", Price = 1.99m }
            );
        }
    }
}
