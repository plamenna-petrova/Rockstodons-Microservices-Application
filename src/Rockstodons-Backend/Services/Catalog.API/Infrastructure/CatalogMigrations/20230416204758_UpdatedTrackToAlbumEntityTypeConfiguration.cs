using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.API.Infrastructure.CatalogMigrations
{
    public partial class UpdatedTrackToAlbumEntityTypeConfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tracks_Albums_AlbumId1",
                table: "Tracks");

            migrationBuilder.DropIndex(
                name: "IX_Tracks_AlbumId1",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "AlbumId1",
                table: "Tracks");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AlbumId1",
                table: "Tracks",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tracks_AlbumId1",
                table: "Tracks",
                column: "AlbumId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Tracks_Albums_AlbumId1",
                table: "Tracks",
                column: "AlbumId1",
                principalTable: "Albums",
                principalColumn: "Id");
        }
    }
}
