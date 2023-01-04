using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DominoAPI.Migrations
{
    /// <inheritdoc />
    public partial class FleetFix02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Volume",
                table: "FuelSupplies",
                newName: "DeliveryVolume");

            migrationBuilder.AddColumn<int>(
                name: "CurrentVolume",
                table: "FuelSupplies",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentVolume",
                table: "FuelSupplies");

            migrationBuilder.RenameColumn(
                name: "DeliveryVolume",
                table: "FuelSupplies",
                newName: "Volume");
        }
    }
}
