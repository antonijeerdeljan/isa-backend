using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISA.Core.Infrastructure.Identity.Migrations
{
    /// <inheritdoc />
    public partial class Init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5310feb4-a1e1-4439-b511-fd2293f33af0"),
                column: "ConcurrencyStamp",
                value: "57afe8fd-dde4-4c6c-a274-e0017bdc53dd");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5310feb4-a1e1-4439-b511-fd2293f33af1"),
                column: "ConcurrencyStamp",
                value: "64bc3332-c457-4a39-94ff-22d546175c20");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5310feb4-a1e1-4439-b511-fd2293f33af2"),
                column: "ConcurrencyStamp",
                value: "cc46b130-f099-40cb-adfc-0ef185d4217a");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5310feb4-a1e1-4439-b511-fd2293f33af0"),
                column: "ConcurrencyStamp",
                value: "d5f7ade0-c361-4b0f-9d5d-b2278a40b6a6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5310feb4-a1e1-4439-b511-fd2293f33af1"),
                column: "ConcurrencyStamp",
                value: "41c4d929-dfe0-4850-aa9a-7a1c54a9683c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5310feb4-a1e1-4439-b511-fd2293f33af2"),
                column: "ConcurrencyStamp",
                value: "46c91935-cb7b-4310-91d2-10dfc455f4bd");
        }
    }
}
