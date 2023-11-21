using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISA.Core.Infrastructure.Identity.Migrations
{
    /// <inheritdoc />
    public partial class nullforrfrtkn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_RefreshToken_refreshTokenId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<Guid>(
                name: "refreshTokenId",
                table: "AspNetUsers",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

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

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_RefreshToken_refreshTokenId",
                table: "AspNetUsers",
                column: "refreshTokenId",
                principalTable: "RefreshToken",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_RefreshToken_refreshTokenId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<Guid>(
                name: "refreshTokenId",
                table: "AspNetUsers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5310feb4-a1e1-4439-b511-fd2293f33af0"),
                column: "ConcurrencyStamp",
                value: "f4bbb0bf-707c-4976-9888-1eb1e91c7538");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5310feb4-a1e1-4439-b511-fd2293f33af1"),
                column: "ConcurrencyStamp",
                value: "5faec716-0dc5-4c7d-a2ce-2a1520563bc3");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5310feb4-a1e1-4439-b511-fd2293f33af2"),
                column: "ConcurrencyStamp",
                value: "bc44210b-235a-4931-a3f8-431ef47b6ed9");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_RefreshToken_refreshTokenId",
                table: "AspNetUsers",
                column: "refreshTokenId",
                principalTable: "RefreshToken",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
