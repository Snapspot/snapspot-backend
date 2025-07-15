using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Snapspot.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSellerPackageCompanyRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SellerPackage_User_UserId",
                table: "SellerPackage");

            migrationBuilder.DropForeignKey(
                name: "FK_SellerPackage_User_UserId1",
                table: "SellerPackage");

            migrationBuilder.DropIndex(
                name: "IX_SellerPackage_UserId",
                table: "SellerPackage");

            migrationBuilder.DropIndex(
                name: "IX_SellerPackage_UserId1",
                table: "SellerPackage");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "SellerPackage");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "SellerPackage");

            migrationBuilder.CreateTable(
                name: "CompanySellerPackage",
                columns: table => new
                {
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SellerPackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanySellerPackage", x => new { x.CompanyId, x.SellerPackageId });
                    table.ForeignKey(
                        name: "FK_CompanySellerPackage_Company_CompaniesId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanySellerPackage_SellerPackage_SellerPackagesId",
                        column: x => x.SellerPackageId,
                        principalTable: "SellerPackage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanySellerPackage_SellerPackagesId",
                table: "CompanySellerPackage",
                column: "SellerPackagesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanySellerPackage");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "SellerPackage",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "SellerPackage",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SellerPackage_UserId",
                table: "SellerPackage",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SellerPackage_UserId1",
                table: "SellerPackage",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_SellerPackage_User_UserId",
                table: "SellerPackage",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SellerPackage_User_UserId1",
                table: "SellerPackage",
                column: "UserId1",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
