using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISA.Application.API.Migrations
{
    /// <inheritdoc />
    public partial class loyaltytable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_LoyaltyProgram_LoyaltyProgramId",
                table: "Customers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LoyaltyProgram",
                table: "LoyaltyProgram");

            migrationBuilder.RenameTable(
                name: "LoyaltyProgram",
                newName: "LoyaltyPrograms");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoyaltyPrograms",
                table: "LoyaltyPrograms",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_LoyaltyPrograms_LoyaltyProgramId",
                table: "Customers",
                column: "LoyaltyProgramId",
                principalTable: "LoyaltyPrograms",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_LoyaltyPrograms_LoyaltyProgramId",
                table: "Customers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LoyaltyPrograms",
                table: "LoyaltyPrograms");

            migrationBuilder.RenameTable(
                name: "LoyaltyPrograms",
                newName: "LoyaltyProgram");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoyaltyProgram",
                table: "LoyaltyProgram",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_LoyaltyProgram_LoyaltyProgramId",
                table: "Customers",
                column: "LoyaltyProgramId",
                principalTable: "LoyaltyProgram",
                principalColumn: "Id");
        }
    }
}
