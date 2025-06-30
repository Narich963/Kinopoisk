using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kinopoisk.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Added_Localization_To_Film_Name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Films_LocalizationSets_DescriptionId",
                table: "Films");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Films");

            migrationBuilder.AlterColumn<int>(
                name: "DescriptionId",
                table: "Films",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "NameId",
                table: "Films",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Films_NameId",
                table: "Films",
                column: "NameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Films_LocalizationSets_DescriptionId",
                table: "Films",
                column: "DescriptionId",
                principalTable: "LocalizationSets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Films_LocalizationSets_NameId",
                table: "Films",
                column: "NameId",
                principalTable: "LocalizationSets",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Films_LocalizationSets_DescriptionId",
                table: "Films");

            migrationBuilder.DropForeignKey(
                name: "FK_Films_LocalizationSets_NameId",
                table: "Films");

            migrationBuilder.DropIndex(
                name: "IX_Films_NameId",
                table: "Films");

            migrationBuilder.DropColumn(
                name: "NameId",
                table: "Films");

            migrationBuilder.AlterColumn<int>(
                name: "DescriptionId",
                table: "Films",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Films",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Films_LocalizationSets_DescriptionId",
                table: "Films",
                column: "DescriptionId",
                principalTable: "LocalizationSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
