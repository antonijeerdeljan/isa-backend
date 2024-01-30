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
                value: "9762ca72-17b7-412b-9e25-1ddfc97cd49b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5310feb4-a1e1-4439-b511-fd2293f33af1"),
                column: "ConcurrencyStamp",
                value: "beac0319-da91-47fa-96ce-1663e197c6c3");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5310feb4-a1e1-4439-b511-fd2293f33af2"),
                column: "ConcurrencyStamp",
                value: "87ed0d5a-4114-42e7-b540-13db7298b72d");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5310feb4-a1e1-4439-b511-fd2293f33af0"),
                column: "ConcurrencyStamp",
                value: "24404f38-ce49-4ccb-82f6-2b8150250e4b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5310feb4-a1e1-4439-b511-fd2293f33af1"),
                column: "ConcurrencyStamp",
                value: "2d009bd6-e963-4a45-a0f1-4be287b789a8");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5310feb4-a1e1-4439-b511-fd2293f33af2"),
                column: "ConcurrencyStamp",
                value: "143b860f-df38-4781-a2e6-29a072daa69e");
        }
    }
}
