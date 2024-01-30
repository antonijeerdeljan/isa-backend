using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISA.Application.API.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    Street = table.Column<string>(type: "text", nullable: true),
                    Number = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoyaltyPrograms",
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
                    table.PrimaryKey("PK_LoyaltyPrograms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    AddressId = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    StartingWorkingHour = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    EndWorkingHour = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    AverageGrade = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Companies_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Firstname = table.Column<string>(type: "text", nullable: false),
                    Lastname = table.Column<string>(type: "text", nullable: false),
                    AddressId = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeliveredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contracts_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Equipments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: true),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Equipments_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CompanyAdmins",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyAdmins", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_CompanyAdmins_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyAdmins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Profession = table.Column<string>(type: "text", nullable: false),
                    CompanyInfo = table.Column<string>(type: "text", nullable: false),
                    LoyaltyProgramId = table.Column<Guid>(type: "uuid", nullable: true),
                    PenaltyPoints = table.Column<int>(type: "integer", nullable: true),
                    Points = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Customers_LoyaltyPrograms_LoyaltyProgramId",
                        column: x => x.LoyaltyProgramId,
                        principalTable: "LoyaltyPrograms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Customers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContractEquipment",
                columns: table => new
                {
                    ContractId = table.Column<Guid>(type: "uuid", nullable: false),
                    EquipmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractEquipment", x => new { x.ContractId, x.EquipmentId });
                    table.ForeignKey(
                        name: "FK_ContractEquipment_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContractEquipment_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Appointment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyAdminUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartingDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndingDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    AlreadyTaken = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointment_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Appointment_CompanyAdmins_CompanyAdminUserId",
                        column: x => x.CompanyAdminUserId,
                        principalTable: "CompanyAdmins",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    AppointmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    State = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.AppointmentId);
                    table.ForeignKey(
                        name: "FK_Reservations_Appointment_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservations_Customers_CustomerUserId",
                        column: x => x.CustomerUserId,
                        principalTable: "Customers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReservationEquipment",
                columns: table => new
                {
                    ReservationId = table.Column<Guid>(type: "uuid", nullable: false),
                    EquipmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationEquipment", x => new { x.ReservationId, x.EquipmentId });
                    table.ForeignKey(
                        name: "FK_ReservationEquipment_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservationEquipment_Reservations_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservations",
                        principalColumn: "AppointmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_CompanyAdminUserId",
                table: "Appointment",
                column: "CompanyAdminUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_CompanyId",
                table: "Appointment",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_AddressId",
                table: "Companies",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyAdmins_CompanyId",
                table: "CompanyAdmins",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractEquipment_EquipmentId",
                table: "ContractEquipment",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_CompanyId",
                table: "Contracts",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_LoyaltyProgramId",
                table: "Customers",
                column: "LoyaltyProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_CompanyId",
                table: "Equipments",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationEquipment_EquipmentId",
                table: "ReservationEquipment",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_CustomerUserId",
                table: "Reservations",
                column: "CustomerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AddressId",
                table: "Users",
                column: "AddressId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContractEquipment");

            migrationBuilder.DropTable(
                name: "ReservationEquipment");

            migrationBuilder.DropTable(
                name: "Contracts");

            migrationBuilder.DropTable(
                name: "Equipments");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Appointment");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "CompanyAdmins");

            migrationBuilder.DropTable(
                name: "LoyaltyPrograms");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Addresses");
        }
    }
}
