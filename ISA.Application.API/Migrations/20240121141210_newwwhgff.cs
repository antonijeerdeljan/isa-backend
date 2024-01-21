using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISA.Application.API.Migrations
{
    /// <inheritdoc />
    public partial class newwwhgff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyAdmins_Users_UserId1",
                table: "CompanyAdmins");

            migrationBuilder.DropIndex(
                name: "IX_CompanyAdmins_UserId1",
                table: "CompanyAdmins");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "CompanyAdmins");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyAdmins_Users_UserId",
                table: "CompanyAdmins",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyAdmins_Users_UserId",
                table: "CompanyAdmins");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "CompanyAdmins",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_CompanyAdmins_UserId1",
                table: "CompanyAdmins",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyAdmins_Users_UserId1",
                table: "CompanyAdmins",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
