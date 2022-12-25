using DominoAPI.Entities.Accounts;
using DominoAPI.Entities.Butchery;
using DominoAPI.Entities.Shops;
using DominoAPI.Entities.Variables;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace DominoAPI.Entities
{
    public class DominoDbContext : DbContext
    {
        private readonly string _connectionString =
            "Server=HAPINGSZEN;Database=DominoDb;Trusted_Connection=True;trustServerCertificate = true;";

        public DbSet<Product> Products { get; set; }
        public DbSet<Carcass> Carcass { get; set; }
        public DbSet<Sausage> Sausages { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ingredient>()
                .HasOne(i => i.Product)
                .WithMany(p => p.Ingredient);

            modelBuilder.Entity<Sale>()
                .HasOne(ss => ss.Shop)
                .WithMany(s => s.Sales)
                .HasForeignKey(ss => ss.ShopId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging(true);
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}