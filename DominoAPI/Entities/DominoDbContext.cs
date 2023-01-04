using DominoAPI.Entities.Accounts;
using DominoAPI.Entities.Butchery;
using DominoAPI.Entities.Shops;
using DominoAPI.Entities.Variables;
using Microsoft.EntityFrameworkCore;
using System.Net;
using DominoAPI.Entities.Fleet;
using Microsoft.EntityFrameworkCore.Diagnostics;
using DominoAPI.Entities.PriceList;

namespace DominoAPI.Entities
{
    public class DominoDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Carcass> Carcass { get; set; }
        public DbSet<Sausage> Sausages { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<MobileShop> MobileShops { get; set; }
        public DbSet<StationaryShop> StationaryShops { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<FuelNote> FuelNotes { get; set; }
        public DbSet<FuelSupply> FuelSupplies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sausage>()
                .HasMany(s => s.Ingredients)
                .WithOne()
                .HasForeignKey(i => i.SausageId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Ingredient>()
                .HasOne(i => i.Product)
                .WithMany(p => p.Ingredient)
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Shop>()
                .HasMany(s => s.Sales)
                .WithOne(ss => ss.Shop)
                .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<MobileShop>()
            //    .HasOne(s => s.Car)
            //    .WithOne(c => c.Shop)
            //    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Car>()
                .HasOne(c => c.Shop)
                .WithOne(s => s.Car)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FuelNote>()
                .HasOne(fn => fn.FuelSupply)
                .WithMany(fs => fs.FuelNotes);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }
    }
}