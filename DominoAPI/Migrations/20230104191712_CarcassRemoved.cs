using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DominoAPI.Migrations
{
    /// <inheritdoc />
    public partial class CarcassRemoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shops_Cars_CarId",
                table: "Shops");

            migrationBuilder.DropTable(
                name: "Carcass");

            migrationBuilder.AddForeignKey(
                name: "FK_Shops_Cars_CarId",
                table: "Shops",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shops_Cars_CarId",
                table: "Shops");

            migrationBuilder.CreateTable(
                name: "Carcass",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Class = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Part = table.Column<int>(type: "int", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductPercentage = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carcass", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carcass_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Carcass_ProductId",
                table: "Carcass",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shops_Cars_CarId",
                table: "Shops",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
