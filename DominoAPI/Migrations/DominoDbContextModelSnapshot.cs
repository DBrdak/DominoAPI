// <auto-generated />
using System;
using DominoAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DominoAPI.Migrations
{
    [DbContext(typeof(DominoDbContext))]
    partial class DominoDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DominoAPI.Entities.Accounts.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("DominoAPI.Entities.Accounts.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DominoAPI.Entities.Butchery.Ingredient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<float>("Content")
                        .HasColumnType("real");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("SausageId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("SausageId");

                    b.ToTable("Ingredients");
                });

            modelBuilder.Entity("DominoAPI.Entities.Butchery.Sausage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<float>("Yield")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("ProductId")
                        .IsUnique();

                    b.ToTable("Sausages");
                });

            modelBuilder.Entity("DominoAPI.Entities.Fleet.Car", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Make")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Mileage")
                        .HasColumnType("int");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Note")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("RegistrationNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Cars");
                });

            modelBuilder.Entity("DominoAPI.Entities.Fleet.FuelNote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CarId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("FuelSupplyId")
                        .HasColumnType("int");

                    b.Property<float>("Volume")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("CarId");

                    b.HasIndex("FuelSupplyId");

                    b.ToTable("FuelNotes");
                });

            modelBuilder.Entity("DominoAPI.Entities.Fleet.FuelSupply", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<float>("CurrentVolume")
                        .HasColumnType("real");

                    b.Property<DateTime>("DateOfDelivery")
                        .HasColumnType("datetime2");

                    b.Property<float>("DeliveryVolume")
                        .HasColumnType("real");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.ToTable("FuelSupplies");
                });

            modelBuilder.Entity("DominoAPI.Entities.PriceList.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.Property<int>("ProductType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("DominoAPI.Entities.Shops.Sale", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Bills")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<float>("SaleAmount")
                        .HasColumnType("real");

                    b.Property<int>("ShopId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ShopId");

                    b.ToTable("Sales");
                });

            modelBuilder.Entity("DominoAPI.Entities.Shops.Shop", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ShopNumber")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Shops");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Shop");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("DominoAPI.Entities.Shops.MobileShop", b =>
                {
                    b.HasBaseType("DominoAPI.Entities.Shops.Shop");

                    b.Property<int>("CarId")
                        .HasColumnType("int");

                    b.HasIndex("CarId")
                        .IsUnique()
                        .HasFilter("[CarId] IS NOT NULL");

                    b.HasDiscriminator().HasValue("MobileShop");
                });

            modelBuilder.Entity("DominoAPI.Entities.Shops.StationaryShop", b =>
                {
                    b.HasBaseType("DominoAPI.Entities.Shops.Shop");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("StationaryShop");
                });

            modelBuilder.Entity("DominoAPI.Entities.Accounts.User", b =>
                {
                    b.HasOne("DominoAPI.Entities.Accounts.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("DominoAPI.Entities.Butchery.Ingredient", b =>
                {
                    b.HasOne("DominoAPI.Entities.PriceList.Product", "Product")
                        .WithMany("Ingredient")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("DominoAPI.Entities.Butchery.Sausage", null)
                        .WithMany("Ingredients")
                        .HasForeignKey("SausageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("DominoAPI.Entities.Butchery.Sausage", b =>
                {
                    b.HasOne("DominoAPI.Entities.PriceList.Product", "Product")
                        .WithOne("Sausage")
                        .HasForeignKey("DominoAPI.Entities.Butchery.Sausage", "ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("DominoAPI.Entities.Fleet.FuelNote", b =>
                {
                    b.HasOne("DominoAPI.Entities.Fleet.Car", "Car")
                        .WithMany("FuelNotes")
                        .HasForeignKey("CarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DominoAPI.Entities.Fleet.FuelSupply", "FuelSupply")
                        .WithMany("FuelNotes")
                        .HasForeignKey("FuelSupplyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Car");

                    b.Navigation("FuelSupply");
                });

            modelBuilder.Entity("DominoAPI.Entities.Shops.Sale", b =>
                {
                    b.HasOne("DominoAPI.Entities.Shops.Shop", "Shop")
                        .WithMany("Sales")
                        .HasForeignKey("ShopId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Shop");
                });

            modelBuilder.Entity("DominoAPI.Entities.Shops.MobileShop", b =>
                {
                    b.HasOne("DominoAPI.Entities.Fleet.Car", "Car")
                        .WithOne("Shop")
                        .HasForeignKey("DominoAPI.Entities.Shops.MobileShop", "CarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Car");
                });

            modelBuilder.Entity("DominoAPI.Entities.Butchery.Sausage", b =>
                {
                    b.Navigation("Ingredients");
                });

            modelBuilder.Entity("DominoAPI.Entities.Fleet.Car", b =>
                {
                    b.Navigation("FuelNotes");

                    b.Navigation("Shop");
                });

            modelBuilder.Entity("DominoAPI.Entities.Fleet.FuelSupply", b =>
                {
                    b.Navigation("FuelNotes");
                });

            modelBuilder.Entity("DominoAPI.Entities.PriceList.Product", b =>
                {
                    b.Navigation("Ingredient");

                    b.Navigation("Sausage");
                });

            modelBuilder.Entity("DominoAPI.Entities.Shops.Shop", b =>
                {
                    b.Navigation("Sales");
                });
#pragma warning restore 612, 618
        }
    }
}
