using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Snapspot.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTimeFieldToSpot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Time",
                table: "Spot",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Time",
                table: "Spot");
        }
    }
}
