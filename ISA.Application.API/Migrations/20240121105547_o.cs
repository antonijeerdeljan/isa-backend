using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISA.Application.API.Migrations
{
    /// <inheritdoc />
    public partial class o : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_CompanyAdmins_Id",
                table: "Users");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "CompanyAdmins",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_CompanyAdmins_UserId",
                table: "CompanyAdmins",
                column: "UserId");

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

            migrationBuilder.DropIndex(
                name: "IX_CompanyAdmins_UserId",
                table: "CompanyAdmins");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CompanyAdmins");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_CompanyAdmins_Id",
                table: "Users",
                column: "Id",
                principalTable: "CompanyAdmins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
