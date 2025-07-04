using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kinopoisk.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Changed_Localization_Entities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Films_Countries_CountryId",
                table: "Films");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Cultures");

            migrationBuilder.DropIndex(
                name: "IX_Films_CountryId",
                table: "Films");

            migrationBuilder.DropColumn(
                name: "Entity",
                table: "LocalizationSets");

            migrationBuilder.DropColumn(
                name: "Property",
                table: "LocalizationSets");

            migrationBuilder.DropColumn(
                name: "CultureInfo",
                table: "Localizations");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Films");

            migrationBuilder.AddColumn<int>(
                name: "Culture",
                table: "Localizations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Property",
                table: "Localizations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Culture",
                table: "Localizations");

            migrationBuilder.DropColumn(
                name: "Property",
                table: "Localizations");

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

            migrationBuilder.AddColumn<string>(
                name: "CultureInfo",
                table: "Localizations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "Films",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameId = table.Column<int>(type: "int", nullable: true),
                    IsoCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Countries_LocalizationSets_NameId",
                        column: x => x.NameId,
                        principalTable: "LocalizationSets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Cultures",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cultures", x => x.Code);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Films_CountryId",
                table: "Films",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_NameId",
                table: "Countries",
                column: "NameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Films_Countries_CountryId",
                table: "Films",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
