using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TaskManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssignedToUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkTasks_Users_AssignedToUserId",
                        column: x => x.AssignedToUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "Name", "Role" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 11, 14, 12, 55, 28, 933, DateTimeKind.Utc).AddTicks(8646), "admin@example.com", "Admin User", "Admin" },
                    { 2, new DateTime(2025, 11, 14, 12, 55, 28, 933, DateTimeKind.Utc).AddTicks(8647), "john@example.com", "John Doe", "User" }
                });

            migrationBuilder.InsertData(
                table: "WorkTasks",
                columns: new[] { "Id", "AssignedToUserId", "CreatedAt", "Description", "Status", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 11, 14, 12, 55, 28, 933, DateTimeKind.Utc).AddTicks(8772), "Initialize database schema", "Completed", "Setup Database", new DateTime(2025, 11, 14, 12, 55, 28, 933, DateTimeKind.Utc).AddTicks(8772) },
                    { 2, 2, new DateTime(2025, 11, 14, 12, 55, 28, 933, DateTimeKind.Utc).AddTicks(8774), "Implement REST API endpoints", "InProgress", "Build API", new DateTime(2025, 11, 14, 12, 55, 28, 933, DateTimeKind.Utc).AddTicks(8775) },
                    { 3, 2, new DateTime(2025, 11, 14, 12, 55, 28, 933, DateTimeKind.Utc).AddTicks(8776), "Build Angular application", "Pending", "Create Frontend", new DateTime(2025, 11, 14, 12, 55, 28, 933, DateTimeKind.Utc).AddTicks(8777) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkTasks_AssignedToUserId",
                table: "WorkTasks",
                column: "AssignedToUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkTasks");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
