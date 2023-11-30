using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ReactWithASP.Server.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
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
                    { new Guid("d63f0ca3-e25d-4583-9354-57f110538a55"), "hashtimemail@gmail.com", "$2a$11$Zizwdtseq053HXE2d95.suv.eVrKy0kxa6qOmGxsGdHbVUYh.8sGS", "mk8ONqTPHcWfq5oXRRWEoZVavZ+Q3vgP2aLy2tIwCX5uHf229sa0or+t6N2MXzlQLCwVX0cNYVRHwWGoSDeNbA==", new DateTime(2023, 11, 30, 6, 5, 37, 596, DateTimeKind.Utc).AddTicks(9399), new DateTime(2023, 11, 30, 6, 35, 37, 596, DateTimeKind.Utc).AddTicks(9393) },
                    { new Guid("d63f0ca3-e25d-4583-9354-57f110538f45"), "dev@tuckerjohnson.me", "$2a$11$LgI.5ktKRsk0QMu.44A14uvOPtpVQGlD7ZnBlh4Y69iC/qOG6Gdv.", "W9SZymTk2DcJ/7wNWMQSWPeqorpj5WKosTDao31WXTWGLw9jfFuJFvmsb/Tn8dGlTuhW5QAb/F7t7lmmMzhIkA==", new DateTime(2023, 11, 30, 6, 5, 37, 495, DateTimeKind.Utc).AddTicks(9720), new DateTime(2023, 11, 30, 6, 35, 37, 495, DateTimeKind.Utc).AddTicks(9710) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Email", "FirstName", "LastName", "Role" },
                values: new object[,]
                {
                    { new Guid("d63f0ca3-e25d-4583-9354-57f110538a55"), "hashtimemail@gmail.com", "Tucker", "Johnson", "Admin" },
                    { new Guid("d63f0ca3-e25d-4583-9354-57f110538f45"), "dev@tuckerjohnson.me", "Tucker", "Johnson", "Admin" }
                });

            migrationBuilder.InsertData(
                table: "ConnectionStrings",
                columns: new[] { "Id", "UserId", "currentTableInteracting", "dbEncryptedConnectionString", "dbName", "dbType" },
                values: new object[,]
                {
                    { new Guid("896d1ec4-e5ff-4354-b7da-2c91453ffcc5"), new Guid("d63f0ca3-e25d-4583-9354-57f110538f45"), "", "host=127.0.0.1; database=HackathonDB; port=5420; user id=postgres; password=123456;", "HackathonDB", "Postgres" },
                    { new Guid("dc6ab834-d615-4d42-979c-41c7226b5f7a"), new Guid("d63f0ca3-e25d-4583-9354-57f110538f45"), "", "host=127.0.0.1; database=exploremoreusa; port=5420; user id=postgres; password=123456;", "ExploreMoreUSA", "Postgres" }
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
