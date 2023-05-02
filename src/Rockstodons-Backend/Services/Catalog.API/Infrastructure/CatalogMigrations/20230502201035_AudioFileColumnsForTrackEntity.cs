using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.API.Infrastructure.CatalogMigrations
{
    public partial class AudioFileColumnsForTrackEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AudioFileName",
                table: "Tracks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AudioFileUrl",
                table: "Tracks",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AudioFileName",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "AudioFileUrl",
                table: "Tracks");
        }
    }
}
