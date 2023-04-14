using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.API.Infrastructure.CatalogMigrations
{
    public partial class AlbumPriceRelatedPropertiesRemoval : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableStock",
                table: "Albums");

            migrationBuilder.DropColumn(
                name: "MaxStockThreshold",
                table: "Albums");

            migrationBuilder.DropColumn(
                name: "OnReorder",
                table: "Albums");

            migrationBuilder.DropColumn(
                name: "PictureFileName",
                table: "Albums");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Albums");

            migrationBuilder.DropColumn(
                name: "RestockThreshold",
                table: "Albums");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AvailableStock",
                table: "Albums",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxStockThreshold",
                table: "Albums",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "OnReorder",
                table: "Albums",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PictureFileName",
                table: "Albums",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Albums",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "RestockThreshold",
                table: "Albums",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
