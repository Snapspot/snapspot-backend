using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Snapspot.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("12d8c894-688a-4b89-bc42-e8b8125dd1fa"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("62d73b9d-7066-4c60-ae5b-9db1dc72b180"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("76536d59-bc50-494f-ad60-ac13b3f80417"));

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedAt", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("1a73f130-b445-4f46-8f88-d4e4a4645e5c"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("2b83f131-b445-4f46-8f88-d4e4a4645e5c"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ThirdParty", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("3c93f132-b445-4f46-8f88-d4e4a4645e5c"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("1a73f130-b445-4f46-8f88-d4e4a4645e5c"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("2b83f131-b445-4f46-8f88-d4e4a4645e5c"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("3c93f132-b445-4f46-8f88-d4e4a4645e5c"));

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedAt", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("12d8c894-688a-4b89-bc42-e8b8125dd1fa"), new DateTime(2025, 5, 28, 15, 55, 32, 67, DateTimeKind.Utc).AddTicks(8952), "User", new DateTime(2025, 5, 28, 15, 55, 32, 67, DateTimeKind.Utc).AddTicks(8952) },
                    { new Guid("62d73b9d-7066-4c60-ae5b-9db1dc72b180"), new DateTime(2025, 5, 28, 15, 55, 32, 67, DateTimeKind.Utc).AddTicks(8949), "ThirdParty", new DateTime(2025, 5, 28, 15, 55, 32, 67, DateTimeKind.Utc).AddTicks(8949) },
                    { new Guid("76536d59-bc50-494f-ad60-ac13b3f80417"), new DateTime(2025, 5, 28, 15, 55, 32, 67, DateTimeKind.Utc).AddTicks(8944), "Admin", new DateTime(2025, 5, 28, 15, 55, 32, 67, DateTimeKind.Utc).AddTicks(8946) }
                });
        }
    }
}
