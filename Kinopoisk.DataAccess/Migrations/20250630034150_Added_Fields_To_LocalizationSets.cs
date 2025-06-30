using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kinopoisk.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Added_Fields_To_LocalizationSets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Entity",
                table: "LocalizationSets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Property",
                table: "LocalizationSets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Entity",
                table: "LocalizationSets");

            migrationBuilder.DropColumn(
                name: "Property",
                table: "LocalizationSets");
        }
    }
}
