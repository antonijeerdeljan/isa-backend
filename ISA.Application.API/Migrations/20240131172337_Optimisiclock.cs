using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISA.Application.API.Migrations
{
    /// <inheritdoc />
    public partial class Optimisiclock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Companies_CompanyId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_CompanyAdmins_CompanyAdminUserId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Appointment_AppointmentId",
                table: "Reservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Appointment",
                table: "Appointment");

            migrationBuilder.RenameTable(
                name: "Appointment",
                newName: "Appointments");

            migrationBuilder.RenameIndex(
                name: "IX_Appointment_CompanyId",
                table: "Appointments",
                newName: "IX_Appointments_CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointment_CompanyAdminUserId",
                table: "Appointments",
                newName: "IX_Appointments_CompanyAdminUserId");

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "Reservations",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "Equipments",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "Appointments",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Appointments",
                table: "Appointments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Companies_CompanyId",
                table: "Appointments",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_CompanyAdmins_CompanyAdminUserId",
                table: "Appointments",
                column: "CompanyAdminUserId",
                principalTable: "CompanyAdmins",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Appointments_AppointmentId",
                table: "Reservations",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Companies_CompanyId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_CompanyAdmins_CompanyAdminUserId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Appointments_AppointmentId",
                table: "Reservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Appointments",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "Appointments");

            migrationBuilder.RenameTable(
                name: "Appointments",
                newName: "Appointment");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_CompanyId",
                table: "Appointment",
                newName: "IX_Appointment_CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_CompanyAdminUserId",
                table: "Appointment",
                newName: "IX_Appointment_CompanyAdminUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Appointment",
                table: "Appointment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Companies_CompanyId",
                table: "Appointment",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_CompanyAdmins_CompanyAdminUserId",
                table: "Appointment",
                column: "CompanyAdminUserId",
                principalTable: "CompanyAdmins",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Appointment_AppointmentId",
                table: "Reservations",
                column: "AppointmentId",
                principalTable: "Appointment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
