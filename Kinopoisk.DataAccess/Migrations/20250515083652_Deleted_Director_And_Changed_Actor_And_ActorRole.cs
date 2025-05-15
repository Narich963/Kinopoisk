using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kinopoisk.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Deleted_Director_And_Changed_Actor_And_ActorRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Films_Directors_DirectorId",
                table: "Films");

            migrationBuilder.DropTable(
                name: "ActorRoles");

            migrationBuilder.DropTable(
                name: "Directors");

            migrationBuilder.DropTable(
                name: "Actors");

            migrationBuilder.DropIndex(
                name: "IX_Films_DirectorId",
                table: "Films");

            migrationBuilder.DropColumn(
                name: "DirectorId",
                table: "Films");

            migrationBuilder.CreateTable(
                name: "FilmEmployees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmEmployees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FilmEmployeeRoles",
                columns: table => new
                {
                    FilmEmployeeID = table.Column<int>(type: "int", nullable: false),
                    FilmId = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    IsDirector = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmEmployeeRoles", x => new { x.FilmEmployeeID, x.FilmId });
                    table.ForeignKey(
                        name: "FK_FilmEmployeeRoles_FilmEmployees_FilmEmployeeID",
                        column: x => x.FilmEmployeeID,
                        principalTable: "FilmEmployees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilmEmployeeRoles_Films_FilmId",
                        column: x => x.FilmId,
                        principalTable: "Films",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FilmEmployeeRoles_FilmId",
                table: "FilmEmployeeRoles",
                column: "FilmId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FilmEmployeeRoles");

            migrationBuilder.DropTable(
                name: "FilmEmployees");

            migrationBuilder.AddColumn<int>(
                name: "DirectorId",
                table: "Films",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Actors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Directors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Directors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ActorRoles",
                columns: table => new
                {
                    ActorId = table.Column<int>(type: "int", nullable: false),
                    FilmId = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActorRoles", x => new { x.ActorId, x.FilmId });
                    table.ForeignKey(
                        name: "FK_ActorRoles_Actors_ActorId",
                        column: x => x.ActorId,
                        principalTable: "Actors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActorRoles_Films_FilmId",
                        column: x => x.FilmId,
                        principalTable: "Films",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Films_DirectorId",
                table: "Films",
                column: "DirectorId");

            migrationBuilder.CreateIndex(
                name: "IX_ActorRoles_FilmId",
                table: "ActorRoles",
                column: "FilmId");

            migrationBuilder.AddForeignKey(
                name: "FK_Films_Directors_DirectorId",
                table: "Films",
                column: "DirectorId",
                principalTable: "Directors",
                principalColumn: "Id");
        }
    }
}
