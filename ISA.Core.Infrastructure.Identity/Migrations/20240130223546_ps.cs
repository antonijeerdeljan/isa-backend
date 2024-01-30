using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISA.Core.Infrastructure.Identity.Migrations
{
    /// <inheritdoc />
    public partial class ps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFirstLogin",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5310feb4-a1e1-4439-b511-fd2293f33af0"),
                column: "ConcurrencyStamp",
                value: "2bf19960-947b-4c7d-b0c2-2a125985bda9");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5310feb4-a1e1-4439-b511-fd2293f33af1"),
                column: "ConcurrencyStamp",
                value: "86c1a356-412b-42eb-aec6-bf2abb592192");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5310feb4-a1e1-4439-b511-fd2293f33af2"),
                column: "ConcurrencyStamp",
                value: "8b2b041c-3c33-416d-9d27-6499734462ac");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFirstLogin",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5310feb4-a1e1-4439-b511-fd2293f33af0"),
                column: "ConcurrencyStamp",
                value: "2dbadc0e-ea7f-40e8-a5ab-ccfa2334aa2a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5310feb4-a1e1-4439-b511-fd2293f33af1"),
                column: "ConcurrencyStamp",
                value: "37ffe2e3-20eb-4b17-9961-ca68383983d8");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5310feb4-a1e1-4439-b511-fd2293f33af2"),
                column: "ConcurrencyStamp",
                value: "4110cb6f-12dd-4e16-b1d4-ba92c9c6d781");
        }
    }
}
