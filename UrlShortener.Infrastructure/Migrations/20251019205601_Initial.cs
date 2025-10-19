using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UrlShortener.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    Username = table.Column<string>(type: "text", nullable: false),
                    NormalizedUsername = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Urls",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginalUrl = table.Column<string>(type: "text", nullable: false),
                    ShortCode = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Urls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Urls_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    RolesId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.RolesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("3be5c12b-adb4-49f1-a10d-bdf648bcb40c"), "Admin" },
                    { new Guid("c10079db-6a58-41d4-b1fc-b6ce4c7d860f"), "User" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "NormalizedUsername", "PasswordHash", "Username" },
                values: new object[,]
                {
                    { new Guid("1bf365a0-3e82-4de8-bea3-f644a541ebc0"), "ADMIN", "admin", "admin" },
                    { new Guid("7d5f1e2c-8f4c-4c3a-9d6a-2e8f4b5c9a1b"), "USER", "user", "user" }
                });

            migrationBuilder.InsertData(
                table: "Urls",
                columns: new[] { "Id", "CreatedById", "CreatedDate", "OriginalUrl", "ShortCode" },
                values: new object[,]
                {
                    { new Guid("814298ac-bc7e-41d4-98b7-0e21db0ae10a"), new Guid("1bf365a0-3e82-4de8-bea3-f644a541ebc0"), new DateTime(2025, 10, 19, 20, 50, 31, 486, DateTimeKind.Utc).AddTicks(4440), "https://localhost:7111/Login", "aDGV9IW" },
                    { new Guid("9aa94be8-cbd6-49ba-831f-061131678d81"), new Guid("7d5f1e2c-8f4c-4c3a-9d6a-2e8f4b5c9a1b"), new DateTime(2025, 10, 19, 20, 50, 31, 486, DateTimeKind.Utc).AddTicks(4440), "http://localhost:5173/user-created", "RZO1LGP" },
                    { new Guid("fdbf0efa-3a81-46ad-a1fd-827ed6183ac2"), new Guid("1bf365a0-3e82-4de8-bea3-f644a541ebc0"), new DateTime(2025, 10, 19, 20, 50, 31, 486, DateTimeKind.Utc).AddTicks(4440), "http://localhost:5173/admin-created", "37f2Fmo" }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RolesId", "UsersId" },
                values: new object[,]
                {
                    { new Guid("3be5c12b-adb4-49f1-a10d-bdf648bcb40c"), new Guid("1bf365a0-3e82-4de8-bea3-f644a541ebc0") },
                    { new Guid("c10079db-6a58-41d4-b1fc-b6ce4c7d860f"), new Guid("7d5f1e2c-8f4c-4c3a-9d6a-2e8f4b5c9a1b") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Urls_CreatedById",
                table: "Urls",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Urls_OriginalUrl",
                table: "Urls",
                column: "OriginalUrl");

            migrationBuilder.CreateIndex(
                name: "IX_Urls_ShortCode",
                table: "Urls",
                column: "ShortCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UsersId",
                table: "UserRoles",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_NormalizedUsername",
                table: "Users",
                column: "NormalizedUsername",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Urls");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
