using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ReactWithASP.Server.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserAccount",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    RefreshToken = table.Column<string>(type: "text", nullable: false),
                    TokenExpires = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TokenCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccount", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "ConnectionStrings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    dbName = table.Column<string>(type: "text", nullable: false),
                    dbType = table.Column<string>(type: "text", nullable: false),
                    dbEncryptedConnectionString = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectionStrings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConnectionStrings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "UserAccount",
                columns: new[] { "UserId", "Email", "PasswordHash", "RefreshToken", "TokenCreated", "TokenExpires" },
                values: new object[,]
                {
                    { new Guid("d63f0ca3-e25d-4583-9354-57f110538a55"), "hashtimemail@gmail.com", "$2a$11$0ncR2tl1T9VecP4ZWimjo.WptHIwYaohQGKsijE5nRrY27eCkUAp6", "bGYfdjWzT4UUmU+Qj7jaJezNkFn6oD5HocvQyzL2o6dgwIYGWhj51HMvr+uWiCrYPECWpULPuKapi4EVhMgEhA==", new DateTime(2023, 11, 27, 19, 57, 56, 971, DateTimeKind.Utc).AddTicks(1258), new DateTime(2023, 11, 27, 20, 27, 56, 971, DateTimeKind.Utc).AddTicks(1252) },
                    { new Guid("d63f0ca3-e25d-4583-9354-57f110538f45"), "dev@tuckerjohnson.me", "$2a$11$xMJRrhZ9T/kxtGk6q2elc.9.8vWbUBFWVNZEbcqQmnVCML7Hm/Vya", "r42JbvvoH9UQFuTB3QV3jaqdrStRET3oI2yi2ldmmIQ6QOsrhQmbhbM+ndya1kAgsgYU0Epxf4SzrKKcwOomfw==", new DateTime(2023, 11, 27, 19, 57, 56, 818, DateTimeKind.Utc).AddTicks(465), new DateTime(2023, 11, 27, 20, 27, 56, 818, DateTimeKind.Utc).AddTicks(458) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Email", "FirstName", "LastName", "Role" },
                values: new object[,]
                {
                    { new Guid("d63f0ca3-e25d-4583-9354-57f110538a55"), "hashtimemail@gmail.com", "Tucker", "Johnson", "Admin" },
                    { new Guid("d63f0ca3-e25d-4583-9354-57f110538f45"), "dev@tuckerjohnson.me", "Tucker", "Johnson", "SuperUser" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConnectionStrings_UserId",
                table: "ConnectionStrings",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConnectionStrings");

            migrationBuilder.DropTable(
                name: "UserAccount");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
