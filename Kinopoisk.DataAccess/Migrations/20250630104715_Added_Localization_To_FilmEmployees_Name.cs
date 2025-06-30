using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kinopoisk.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Added_Localization_To_FilmEmployees_Name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "FilmEmployees");

            migrationBuilder.AddColumn<int>(
                name: "NameId",
                table: "FilmEmployees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FilmEmployees_NameId",
                table: "FilmEmployees",
                column: "NameId");

            migrationBuilder.AddForeignKey(
                name: "FK_FilmEmployees_LocalizationSets_NameId",
                table: "FilmEmployees",
                column: "NameId",
                principalTable: "LocalizationSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FilmEmployees_LocalizationSets_NameId",
                table: "FilmEmployees");

            migrationBuilder.DropIndex(
                name: "IX_FilmEmployees_NameId",
                table: "FilmEmployees");

            migrationBuilder.DropColumn(
                name: "NameId",
                table: "FilmEmployees");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "FilmEmployees",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
