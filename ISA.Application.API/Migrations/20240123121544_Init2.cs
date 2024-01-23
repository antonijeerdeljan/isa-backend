using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISA.Application.API.Migrations
{
    /// <inheritdoc />
    public partial class Init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_CompanyAdmins_CompanyAdminId",
                table: "Appointment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanyAdmins",
                table: "CompanyAdmins");

            migrationBuilder.DropIndex(
                name: "IX_CompanyAdmins_UserId",
                table: "CompanyAdmins");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CompanyAdmins");

            migrationBuilder.RenameColumn(
                name: "CompanyAdminId",
                table: "Appointment",
                newName: "CompanyAdminUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointment_CompanyAdminId",
                table: "Appointment",
                newName: "IX_Appointment_CompanyAdminUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanyAdmins",
                table: "CompanyAdmins",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_CompanyAdmins_CompanyAdminUserId",
                table: "Appointment",
                column: "CompanyAdminUserId",
                principalTable: "CompanyAdmins",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_CompanyAdmins_CompanyAdminUserId",
                table: "Appointment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanyAdmins",
                table: "CompanyAdmins");

            migrationBuilder.RenameColumn(
                name: "CompanyAdminUserId",
                table: "Appointment",
                newName: "CompanyAdminId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointment_CompanyAdminUserId",
                table: "Appointment",
                newName: "IX_Appointment_CompanyAdminId");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "CompanyAdmins",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanyAdmins",
                table: "CompanyAdmins",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyAdmins_UserId",
                table: "CompanyAdmins",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_CompanyAdmins_CompanyAdminId",
                table: "Appointment",
                column: "CompanyAdminId",
                principalTable: "CompanyAdmins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
