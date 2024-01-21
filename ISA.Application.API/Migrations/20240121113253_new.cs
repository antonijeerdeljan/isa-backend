using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISA.Application.API.Migrations
{
    /// <inheritdoc />
    public partial class @new : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminFirstName",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "AdminLastName",
                table: "Appointment");

            migrationBuilder.RenameColumn(
                name: "AdminId",
                table: "Appointment",
                newName: "CompanyAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_CompanyAdminId",
                table: "Appointment",
                column: "CompanyAdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_CompanyAdmins_CompanyAdminId",
                table: "Appointment",
                column: "CompanyAdminId",
                principalTable: "CompanyAdmins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_CompanyAdmins_CompanyAdminId",
                table: "Appointment");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_CompanyAdminId",
                table: "Appointment");

            migrationBuilder.RenameColumn(
                name: "CompanyAdminId",
                table: "Appointment",
                newName: "AdminId");

            migrationBuilder.AddColumn<string>(
                name: "AdminFirstName",
                table: "Appointment",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AdminLastName",
                table: "Appointment",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
