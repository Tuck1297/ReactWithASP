using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ReactWithASP.Server.Migrations.Data
{
    /// <inheritdoc />
    public partial class InitialApiContext : Migration
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
                    dbConnectionString = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    currentTableInteracting = table.Column<string>(type: "text", nullable: false)
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
                    { new Guid("d63f0ca3-e25d-4583-9354-57f110538a55"), "test2hackathon.com", "$2a$11$uramN.Jwempnqmbt.T/aVeHnWiShr9VtcP3qC3mY5Y2eD2bBuiPYW", "4LlQcMOp/jizlaolyy029YtgVsEFAnQ9iYnd0MHIlDyJca53XNwbLVGzeSe9XkFFBjcUqGs5t1TQzyDeE/7J/A==", new DateTime(2023, 12, 2, 2, 16, 44, 948, DateTimeKind.Utc).AddTicks(1797), new DateTime(2023, 12, 2, 2, 46, 44, 948, DateTimeKind.Utc).AddTicks(1789) },
                    { new Guid("d63f0ca3-e25d-4583-9354-57f110538f45"), "test1@hackathon.com", "$2a$11$q1udDZHzUH2s37H.WNEmVupM/0siYeHWWJ.zYIS40r34mDGV1kd0i", "J19S5PwCdOsc152kT2T0r9Hx5WEBKTo6rNbm3GVTGAwdAsDfvGQcOHzxBe+qt7ls8S1SlyvT5VtFWUKKquAfMQ==", new DateTime(2023, 12, 2, 2, 16, 44, 848, DateTimeKind.Utc).AddTicks(696), new DateTime(2023, 12, 2, 2, 46, 44, 848, DateTimeKind.Utc).AddTicks(691) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Email", "FirstName", "LastName", "Role" },
                values: new object[,]
                {
                    { new Guid("d63f0ca3-e25d-4583-9354-57f110538a55"), "test2@hackathon.com", "TestUser", "Tester", "User" },
                    { new Guid("d63f0ca3-e25d-4583-9354-57f110538f45"), "test1@hackathon.com", "TestAdmin", "Tester", "Admin" }
                });

            migrationBuilder.InsertData(
                table: "ConnectionStrings",
                columns: new[] { "Id", "UserId", "currentTableInteracting", "dbConnectionString", "dbName", "dbType" },
                values: new object[,]
                {
                    { new Guid("3740e256-5da4-454b-aac9-51a45eaac97a"), new Guid("d63f0ca3-e25d-4583-9354-57f110538f45"), "", "host=127.0.0.1; database=WebsiteInfo; port=5420; user id=postgres; password=123456;", "WebsiteInfo", "Postgres" },
                    { new Guid("8e2a76d6-a6d2-4fcf-a742-b878a5da2b83"), new Guid("d63f0ca3-e25d-4583-9354-57f110538f45"), "", "host=127.0.0.1; database=SupplyChain; port=5420; user id=postgres; password=123456;", "SupplyChain", "Postgres" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConnectionStrings_UserId",
                table: "ConnectionStrings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectionStrings_dbName",
                table: "ConnectionStrings",
                column: "dbName",
                unique: true);
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
