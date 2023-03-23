using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.API.Infrastructure.CatalogMigrations
{
    public partial class PerformerInitialSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PerformerId",
                table: "Genres",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PerformerId",
                table: "Albums",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Performer",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    History = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Performer", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Genres_PerformerId",
                table: "Genres",
                column: "PerformerId");

            migrationBuilder.CreateIndex(
                name: "IX_Albums_PerformerId",
                table: "Albums",
                column: "PerformerId");

            migrationBuilder.CreateIndex(
                name: "IX_Performer_IsDeleted",
                table: "Performer",
                column: "IsDeleted");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Albums_Performer_PerformerId",
                table: "Albums");

            migrationBuilder.DropForeignKey(
                name: "FK_Genres_Performer_PerformerId",
                table: "Genres");

            migrationBuilder.DropTable(
                name: "Performer");

            migrationBuilder.DropIndex(
                name: "IX_Genres_PerformerId",
                table: "Genres");

            migrationBuilder.DropIndex(
                name: "IX_Albums_PerformerId",
                table: "Albums");

            migrationBuilder.DropColumn(
                name: "PerformerId",
                table: "Genres");

            migrationBuilder.DropColumn(
                name: "PerformerId",
                table: "Albums");
        }
    }
}
