using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISA.Application.API.Migrations
{
    /// <inheritdoc />
    public partial class loyalty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LoyaltyProgramId",
                table: "Customers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PenaltyPoints",
                table: "Customers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Points",
                table: "Customers",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LoyaltyProgram",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    NewPoints = table.Column<int>(type: "integer", nullable: false),
                    MinCategoryThresholds = table.Column<int>(type: "integer", nullable: false),
                    MaxCategoryThresholds = table.Column<int>(type: "integer", nullable: false),
                    MaxPenaltyPoints = table.Column<int>(type: "integer", nullable: false),
                    CategoryDiscounts = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoyaltyProgram", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_LoyaltyProgramId",
                table: "Customers",
                column: "LoyaltyProgramId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_LoyaltyProgram_LoyaltyProgramId",
                table: "Customers",
                column: "LoyaltyProgramId",
                principalTable: "LoyaltyProgram",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_LoyaltyProgram_LoyaltyProgramId",
                table: "Customers");

            migrationBuilder.DropTable(
                name: "LoyaltyProgram");

            migrationBuilder.DropIndex(
                name: "IX_Customers_LoyaltyProgramId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "LoyaltyProgramId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "PenaltyPoints",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Points",
                table: "Customers");
        }
    }
}
