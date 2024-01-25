using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISA.Application.API.Migrations
{
    /// <inheritdoc />
    public partial class reskey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservationEquipment_Reservations_ReservationId",
                table: "ReservationEquipment");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Appointment_AppointmentId",
                table: "Reservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reservations",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_AppointmentId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "AppointmentId",
                table: "Reservations",
                newName: "AppointmentID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reservations",
                table: "Reservations",
                column: "AppointmentID");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationEquipment_Reservations_ReservationId",
                table: "ReservationEquipment",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "AppointmentID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Appointment_AppointmentID",
                table: "Reservations",
                column: "AppointmentID",
                principalTable: "Appointment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservationEquipment_Reservations_ReservationId",
                table: "ReservationEquipment");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Appointment_AppointmentID",
                table: "Reservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reservations",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "AppointmentID",
                table: "Reservations",
                newName: "AppointmentId");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Reservations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reservations",
                table: "Reservations",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_AppointmentId",
                table: "Reservations",
                column: "AppointmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationEquipment_Reservations_ReservationId",
                table: "ReservationEquipment",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
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
