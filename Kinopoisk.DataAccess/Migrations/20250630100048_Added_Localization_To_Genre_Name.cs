using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kinopoisk.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Added_Localization_To_Genre_Name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Genres");

            migrationBuilder.AddColumn<int>(
                name: "NameId",
                table: "Genres",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Genres_NameId",
                table: "Genres",
                column: "NameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Genres_LocalizationSets_NameId",
                table: "Genres",
                column: "NameId",
                principalTable: "LocalizationSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Genres_LocalizationSets_NameId",
                table: "Genres");

            migrationBuilder.DropIndex(
                name: "IX_Genres_NameId",
                table: "Genres");

            migrationBuilder.DropColumn(
                name: "NameId",
                table: "Genres");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Genres",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
