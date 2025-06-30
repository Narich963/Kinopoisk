using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kinopoisk.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Added_Localization_To_Country_Name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Countries");

            migrationBuilder.AddColumn<int>(
                name: "NameId",
                table: "Countries",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_NameId",
                table: "Countries",
                column: "NameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Countries_LocalizationSets_NameId",
                table: "Countries",
                column: "NameId",
                principalTable: "LocalizationSets",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Countries_LocalizationSets_NameId",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Countries_NameId",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "NameId",
                table: "Countries");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Countries",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
