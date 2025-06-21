using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Snapspot.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixUserId1Relationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_User_UserId1",
                table: "Feedback");

            migrationBuilder.DropIndex(
                name: "IX_Feedback_UserId1",
                table: "Feedback");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Feedback");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "Feedback",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_UserId1",
                table: "Feedback",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_User_UserId1",
                table: "Feedback",
                column: "UserId1",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
