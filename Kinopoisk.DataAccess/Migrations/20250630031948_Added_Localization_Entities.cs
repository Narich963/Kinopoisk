using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kinopoisk.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Added_Localization_Entities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Films");

            migrationBuilder.AddColumn<int>(
                name: "DescriptionId",
                table: "Films",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.CreateTable(
                name: "LocalizationSets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalizationSets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Localizations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CultureInfo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocalizationSetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Localizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Localizations_LocalizationSets_LocalizationSetId",
                        column: x => x.LocalizationSetId,
                        principalTable: "LocalizationSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Films_DescriptionId",
                table: "Films",
                column: "DescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Localizations_LocalizationSetId",
                table: "Localizations",
                column: "LocalizationSetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Films_LocalizationSets_DescriptionId",
                table: "Films",
                column: "DescriptionId",
                principalTable: "LocalizationSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Films_LocalizationSets_DescriptionId",
                table: "Films");

            migrationBuilder.DropTable(
                name: "Cultures");

            migrationBuilder.DropTable(
                name: "Localizations");

            migrationBuilder.DropTable(
                name: "LocalizationSets");

            migrationBuilder.DropIndex(
                name: "IX_Films_DescriptionId",
                table: "Films");

            migrationBuilder.DropColumn(
                name: "DescriptionId",
                table: "Films");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Films",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }
    }
}
