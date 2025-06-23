using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Snapspot.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserId1FromFeedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Company_User_UserId1",
                table: "Company");

            migrationBuilder.DropIndex(
                name: "IX_Company_UserId1",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Company");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "Company",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Company_UserId1",
                table: "Company",
                column: "UserId1",
                unique: true,
                filter: "[UserId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Company_User_UserId1",
                table: "Company",
                column: "UserId1",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
