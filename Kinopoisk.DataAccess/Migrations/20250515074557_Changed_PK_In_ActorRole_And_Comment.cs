using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kinopoisk.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Changed_PK_In_ActorRole_And_Comment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_UserId",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActorRoles",
                table: "ActorRoles");

            migrationBuilder.DropIndex(
                name: "IX_ActorRoles_ActorId",
                table: "ActorRoles");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ActorRoles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                columns: new[] { "UserId", "FilmId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActorRoles",
                table: "ActorRoles",
                columns: new[] { "ActorId", "FilmId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActorRoles",
                table: "ActorRoles");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ActorRoles",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActorRoles",
                table: "ActorRoles",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ActorRoles_ActorId",
                table: "ActorRoles",
                column: "ActorId");
        }
    }
}
