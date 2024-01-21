using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISA.Application.API.Migrations
{
    /// <inheritdoc />
    public partial class newwwhgf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Users_CompanyAdminId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Companies_CompanyId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_UserId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CompanyId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "CompanyAdminId",
                table: "Appointment",
                newName: "CompanyAdminUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointment_CompanyAdminId",
                table: "Appointment",
                newName: "IX_Appointment_CompanyAdminUserId");

            migrationBuilder.CreateTable(
                name: "CompanyAdmins",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId1 = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyAdmins", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_CompanyAdmins_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyAdmins_Users_UserId1",
                        column: x => x.UserId1,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyAdmins_CompanyId",
                table: "CompanyAdmins",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyAdmins_UserId1",
                table: "CompanyAdmins",
                column: "UserId1");

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

            migrationBuilder.DropTable(
                name: "CompanyAdmins");

            migrationBuilder.RenameColumn(
                name: "CompanyAdminUserId",
                table: "Appointment",
                newName: "CompanyAdminId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointment_CompanyAdminUserId",
                table: "Appointment",
                newName: "IX_Appointment_CompanyAdminId");

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_CompanyId",
                table: "Users",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserId",
                table: "Users",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Users_CompanyAdminId",
                table: "Appointment",
                column: "CompanyAdminId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Companies_CompanyId",
                table: "Users",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_UserId",
                table: "Users",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
