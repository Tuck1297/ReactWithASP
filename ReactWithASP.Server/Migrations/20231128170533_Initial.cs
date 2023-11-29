using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ReactWithASP.Server.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
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
                    { new Guid("d63f0ca3-e25d-4583-9354-57f110538a55"), "hashtimemail@gmail.com", "$2a$11$r8mZQDkgMmVn3/I.oa6mcuZpGuHAvUi7BvrgiV1bIKx750JA0gYGq", "2wIuenT4Dk6lhMukbEb6iBmd4Y+S2n103SNXuxwzh8EKHfHpNMr6r0UIk2+EHXRUb89c+ON+JfgtF/kN+N7UYQ==", new DateTime(2023, 11, 28, 17, 5, 33, 787, DateTimeKind.Utc).AddTicks(6475), new DateTime(2023, 11, 28, 17, 35, 33, 787, DateTimeKind.Utc).AddTicks(6470) },
                    { new Guid("d63f0ca3-e25d-4583-9354-57f110538f45"), "dev@tuckerjohnson.me", "$2a$11$.uLwuTWjCMnTaXborDtiYO25N4EiBGWXiAER6iT9wjGyzJvFtfPSi", "UdaLvOn1W+GJsTxjQw4auzp+gwb8QCUp8Zmme6FfvMggOkutoLe1GvlKUivBT2aspLzFCGzCEphk9zCmDVaK0g==", new DateTime(2023, 11, 28, 17, 5, 33, 687, DateTimeKind.Utc).AddTicks(7558), new DateTime(2023, 11, 28, 17, 35, 33, 687, DateTimeKind.Utc).AddTicks(7551) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Email", "FirstName", "LastName", "Role" },
                values: new object[,]
                {
                    { new Guid("d63f0ca3-e25d-4583-9354-57f110538a55"), "hashtimemail@gmail.com", "Tucker", "Johnson", "Admin" },
                    { new Guid("d63f0ca3-e25d-4583-9354-57f110538f45"), "dev@tuckerjohnson.me", "Tucker", "Johnson", "Admin" }
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
