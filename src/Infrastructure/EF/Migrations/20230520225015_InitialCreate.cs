using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ELibrary_UserService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "userService");

            migrationBuilder.CreateTable(
                name: "Book",
                schema: "userService",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Book", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "userService",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    AmountToPay = table.Column<decimal>(type: "numeric", nullable: false),
                    Description = table.Column<string>(type: "character varying(10000)", maxLength: 10000, nullable: true),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    IsAccountBlocked = table.Column<bool>(type: "boolean", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookUser",
                schema: "userService",
                columns: table => new
                {
                    UsersId = table.Column<string>(type: "text", nullable: false),
                    WatchListId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookUser", x => new { x.UsersId, x.WatchListId });
                    table.ForeignKey(
                        name: "FK_BookUser_Book_WatchListId",
                        column: x => x.WatchListId,
                        principalSchema: "userService",
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookUser_User_UsersId",
                        column: x => x.UsersId,
                        principalSchema: "userService",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookUser_WatchListId",
                schema: "userService",
                table: "BookUser",
                column: "WatchListId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookUser",
                schema: "userService");

            migrationBuilder.DropTable(
                name: "Book",
                schema: "userService");

            migrationBuilder.DropTable(
                name: "User",
                schema: "userService");
        }
    }
}
