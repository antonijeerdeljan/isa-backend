using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISA.Application.API.Migrations
{
    /// <inheritdoc />
    public partial class RemovedAddressId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Addresses_AddresId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_AddresId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "AddresId",
                table: "Users",
                newName: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AddressId",
                table: "Users",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Addresses_AddressId",
                table: "Users",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Addresses_AddressId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_AddressId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "AddressId",
                table: "Users",
                newName: "AddresId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AddresId",
                table: "Users",
                column: "AddresId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Addresses_AddresId",
                table: "Users",
                column: "AddresId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
