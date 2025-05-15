using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kinopoisk.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Added_Countries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "Films");

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
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsoCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Films_CountryId",
                table: "Films",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Films_Countries_CountryId",
                table: "Films",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Films_Countries_CountryId",
                table: "Films");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Films_CountryId",
                table: "Films");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Films");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Films",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
