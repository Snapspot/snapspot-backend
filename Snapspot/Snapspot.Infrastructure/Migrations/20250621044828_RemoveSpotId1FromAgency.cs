using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Snapspot.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSpotId1FromAgency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agency_Spot_SpotId1",
                table: "Agency");

            migrationBuilder.DropIndex(
                name: "IX_Agency_SpotId1",
                table: "Agency");

            migrationBuilder.DropColumn(
                name: "SpotId1",
                table: "Agency");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SpotId1",
                table: "Agency",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Agency_SpotId1",
                table: "Agency",
                column: "SpotId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Agency_Spot_SpotId1",
                table: "Agency",
                column: "SpotId1",
                principalTable: "Spot",
                principalColumn: "Id");
        }
    }
}
