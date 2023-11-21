using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISA.Core.Infrastructure.Identity.Migrations
{
    /// <inheritdoc />
    public partial class rfrtktk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_RefreshToken_refreshTokenId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_refreshTokenId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "refreshTokenId",
                table: "AspNetUsers",
                newName: "RefreshToken");

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpirationDate",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5310feb4-a1e1-4439-b511-fd2293f33af0"),
                column: "ConcurrencyStamp",
                value: "4bc87861-3f18-4cdd-95c2-d56324ce2410");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5310feb4-a1e1-4439-b511-fd2293f33af1"),
                column: "ConcurrencyStamp",
                value: "f4b8cd6c-68a0-4b09-a46a-452b79516334");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5310feb4-a1e1-4439-b511-fd2293f33af2"),
                column: "ConcurrencyStamp",
                value: "ecd47019-2af2-47c0-8c15-62c21c355629");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshTokenExpirationDate",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "RefreshToken",
                table: "AspNetUsers",
                newName: "refreshTokenId");

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5310feb4-a1e1-4439-b511-fd2293f33af0"),
                column: "ConcurrencyStamp",
                value: "0aabaf15-e484-41bd-851d-177612508ad3");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5310feb4-a1e1-4439-b511-fd2293f33af1"),
                column: "ConcurrencyStamp",
                value: "c16e5e84-70d9-4ec9-b288-df9e7872e6b4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5310feb4-a1e1-4439-b511-fd2293f33af2"),
                column: "ConcurrencyStamp",
                value: "b508a2cd-39ee-451e-9aba-b7cc319152a0");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_refreshTokenId",
                table: "AspNetUsers",
                column: "refreshTokenId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_RefreshToken_refreshTokenId",
                table: "AspNetUsers",
                column: "refreshTokenId",
                principalTable: "RefreshToken",
                principalColumn: "Id");
        }
    }
}
