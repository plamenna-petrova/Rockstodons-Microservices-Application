using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.API.Infrastructure.CatalogMigrations
{
    public partial class PerformersTableRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Albums_Performer_PerformerId",
                table: "Albums");

            migrationBuilder.DropForeignKey(
                name: "FK_Genres_Performer_PerformerId",
                table: "Genres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Performer",
                table: "Performer");

            migrationBuilder.RenameTable(
                name: "Performer",
                newName: "Performers");

            migrationBuilder.RenameIndex(
                name: "IX_Performer_IsDeleted",
                table: "Performers",
                newName: "IX_Performers_IsDeleted");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Performers",
                table: "Performers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Albums_Performers_PerformerId",
                table: "Albums",
                column: "PerformerId",
                principalTable: "Performers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Genres_Performers_PerformerId",
                table: "Genres",
                column: "PerformerId",
                principalTable: "Performers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Albums_Performers_PerformerId",
                table: "Albums");

            migrationBuilder.DropForeignKey(
                name: "FK_Genres_Performers_PerformerId",
                table: "Genres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Performers",
                table: "Performers");

            migrationBuilder.RenameTable(
                name: "Performers",
                newName: "Performer");

            migrationBuilder.RenameIndex(
                name: "IX_Performers_IsDeleted",
                table: "Performer",
                newName: "IX_Performer_IsDeleted");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Performer",
                table: "Performer",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Albums_Performer_PerformerId",
                table: "Albums",
                column: "PerformerId",
                principalTable: "Performer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Genres_Performer_PerformerId",
                table: "Genres",
                column: "PerformerId",
                principalTable: "Performer",
                principalColumn: "Id");
        }
    }
}
