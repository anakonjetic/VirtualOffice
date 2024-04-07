using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtualOffice.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EvaluationType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvaluationType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequestType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Equipment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    EquipmentCategoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Equipment_EquipmentCategory_EquipmentCategoryId",
                        column: x => x.EquipmentCategoryId,
                        principalTable: "EquipmentCategory",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RemainingDaysOff = table.Column<int>(type: "int", nullable: false),
                    SickLeaveDaysUsed = table.Column<int>(type: "int", nullable: false),
                    EquipmentId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TeamId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employee_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClockIns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClockInTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClockOutTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClockIns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClockIns_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeManager",
                columns: table => new
                {
                    ManagerId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeManager", x => new { x.ManagerId, x.EmployeeId });
                    table.ForeignKey(
                        name: "FK_EmployeeManager_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeManager_Employee_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EvaluationForm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    ManagerId = table.Column<int>(type: "int", nullable: false),
                    FormTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FormDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EvaluationTypeId = table.Column<int>(type: "int", nullable: false),
                    EvaluationTypeId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvaluationForm", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvaluationForm_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvaluationForm_Employee_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvaluationForm_EvaluationType_EvaluationTypeId",
                        column: x => x.EvaluationTypeId,
                        principalTable: "EvaluationType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvaluationForm_EvaluationType_EvaluationTypeId1",
                        column: x => x.EvaluationTypeId1,
                        principalTable: "EvaluationType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Request",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ManagerId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    RequestTypeID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Request", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Request_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Request_Employee_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Employee",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Request_RequestType_RequestTypeID",
                        column: x => x.RequestTypeID,
                        principalTable: "RequestType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Request_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "1e95f075-9cbd-4252-8a25-faeb03e0449e", 0, "4f6b5901-3484-4193-8019-eda79b7bb7c4", "akonjetic@tvz.hr", false, true, null, "AKONJETIC@TVZ.HR", "AKONJETIC@TVZ.HR", "AQAAAAEAACcQAAAAEHdoVZxob+hAFS7k94sF73hok2cbxnwffaz2Lh64SLJjSL4RXBfMEOhRYa4FfNRbgw==", null, false, "DLH4EV4JMJXZHL7E26I56PEVVUUTTIHP", false, "akonjetic@tvz.hr" },
                    { "938ea5fe-88c8-4662-9192-c3c668a7cb07", 0, "f3cd1583-e393-40cc-8bdd-c31d0b94e9ca", "lkranjcec@tvz.hr", false, true, null, "LKRANJCEC@TVZ.HR", "LKRANJCEC@TVZ.HR", "AQAAAAEAACcQAAAAEAOqiEegzbkuyHFSWeQmTidH+MrCH86ckhw1q9ellMBVEL7LgaH+6OpeCiX+Dk7AXw==", null, false, "UQGAQP2JVWKWF2FF4P44UT5WLZDAZCSX", false, "lkranjcec@tvz.hr" },
                    { "9ad46335-32b4-492d-8592-4379e0f2f108", 0, "24c9aa1d-9f2a-46a0-86e1-a243019622b6", "mtkalec@tvz.hr", false, true, null, "MTKALEC@TVZ.HR", "MTKALEC@TVZ.HR", "AQAAAAEAACcQAAAAECk7W8s/aNRCGsRYsTy4OFTL6m6UtZ24akpO+00ixeCfe5jOnRa2RaUgTCgdLqE+wQ==", null, false, "2P3O5IYYVMYAJPTP4SJBNZCKWICKIUFJ", false, "mtkalec@tvz.hr" },
                    { "c74eba9f-b845-4a07-b524-16333e5d0a28", 0, "a000153f-27b3-4f1a-9239-54a0da24b5ec", "lradosev1@tvz.hr", false, true, null, "LRADOSEV1@TVZ.HR", "LRADOSEV1@TVZ.HR", "AQAAAAEAACcQAAAAEI8AdUEzjlWIzWCDg9b1Pxts7gJRnbdF40Z2g85lHfejyCT+iNuffZfCRphOahvbWw==", null, false, "6NMWXOKUSY6BAUYR5AQXNLMCAF3GS5F6", false, "lradosev1@tvz.hr" },
                    { "f0e2bb7d-9ef4-421d-ad33-850f079c9507", 0, "a097299b-84ec-4ac4-a12e-f29c46f83610", "ijelinic@tvz.hr", false, true, null, "IJELINIC@TVZ.HR", "IJELINIC@TVZ.HR", "AQAAAAEAACcQAAAAEDWL7FHTTTSozek+8JSsLrZVKVgj1weRDzOIbyHFxCdt2ql3B/aV21aAW36GbV3wRw==", null, false, "XGHG2H7ZFL7VBGKKCEK7F62YQH7JCJP7", false, "ijelinic@tvz.hr" }
                });

            migrationBuilder.InsertData(
                table: "Equipment",
                columns: new[] { "Id", "CategoryId", "EquipmentCategoryId", "Name" },
                values: new object[,]
                {
                    { 1, 1, null, "Laptop HP" },
                    { 2, 1, null, "Laptop Macbook" },
                    { 3, 1, null, "Laptop Lenovo" },
                    { 4, 5, null, "Mouse Logitech" },
                    { 5, 5, null, "Mouse Apple" },
                    { 6, 5, null, "Keyboard Logitech" }
                });

            migrationBuilder.InsertData(
                table: "EquipmentCategory",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Computers" },
                    { 2, "Printers and scanners" },
                    { 3, "Telecommunication" },
                    { 4, "Presentation" },
                    { 5, "Computer peripherals" }
                });

            migrationBuilder.InsertData(
                table: "Team",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Development" },
                    { 2, "Marketing" },
                    { 3, "Finance" },
                    { 4, "Retail" },
                    { 5, "Administration" }
                });

            migrationBuilder.InsertData(
                table: "Employee",
                columns: new[] { "Id", "DateOfBirth", "EquipmentId", "FirstName", "LastName", "RemainingDaysOff", "SickLeaveDaysUsed", "TeamId", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2000, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "1#2#3", "Ana", "Konjetić", 20, 5, 1, "akonjetic@tvz.hr" },
                    { 2, new DateTime(1997, 2, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "4#5#6", "Marko", "Tkalec", 15, 2, 2, "mtkalec@tvz.hr" },
                    { 3, new DateTime(2000, 6, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "1#3#6", "Ivan", "Jelinić", 15, 2, 3, "ijelinic@tvz.hr" },
                    { 4, new DateTime(1985, 9, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "1#4#5", "Marina", "Marković", 17, 2, 1, "" },
                    { 5, new DateTime(1995, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "2#3#6", "Ivan", "Babić", 19, 3, 2, "" },
                    { 6, new DateTime(1993, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "1#3#5", "Ana", "Knežević", 15, 2, 3, "" },
                    { 7, new DateTime(1987, 4, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "2#4#6", "Petar", "Petrović", 18, 3, 1, "" },
                    { 8, new DateTime(1991, 7, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "3#5#6", "Martina", "Šimunović", 16, 2, 2, "" },
                    { 9, new DateTime(1989, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "1#4#5", "Antonio", "Vuković", 20, 1, 3, "" },
                    { 10, new DateTime(1994, 10, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "2#3#6", "Jelena", "Matković", 17, 2, 1, "" },
                    { 11, new DateTime(1988, 8, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "2#4#6", "Ivana", "Horvat", 16, 3, 2, "" },
                    { 12, new DateTime(1992, 3, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "3#5#6", "Luka", "Kovač", 20, 1, 3, "" },
                    { 13, new DateTime(1990, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "1#2#3", "Ante", "Kovačić", 18, 4, 1, "" },
                    { 14, new DateTime(2001, 5, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "4#5#6", "Lucija", "Kranjčec", 25, 1, 5, "lkranjcec@tvz.hr" },
                    { 15, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "1#2#3", "Luka", "Radošević", 12, 10, 4, "lradosev1@tvz.hr" }
                });

            migrationBuilder.InsertData(
                table: "EmployeeManager",
                columns: new[] { "EmployeeId", "ManagerId" },
                values: new object[,]
                {
                    { 4, 1 },
                    { 7, 1 },
                    { 10, 1 },
                    { 13, 1 },
                    { 5, 2 },
                    { 8, 2 },
                    { 11, 2 },
                    { 6, 3 },
                    { 9, 3 },
                    { 12, 3 },
                    { 1, 14 },
                    { 2, 14 },
                    { 3, 14 },
                    { 15, 14 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ClockIns_EmployeeId",
                table: "ClockIns",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_TeamId",
                table: "Employee",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeManager_EmployeeId",
                table: "EmployeeManager",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_EquipmentCategoryId",
                table: "Equipment",
                column: "EquipmentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationForm_EmployeeId",
                table: "EvaluationForm",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationForm_EvaluationTypeId",
                table: "EvaluationForm",
                column: "EvaluationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationForm_EvaluationTypeId1",
                table: "EvaluationForm",
                column: "EvaluationTypeId1");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationForm_ManagerId",
                table: "EvaluationForm",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Request_EmployeeId",
                table: "Request",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Request_ManagerId",
                table: "Request",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Request_RequestTypeID",
                table: "Request",
                column: "RequestTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Request_StatusId",
                table: "Request",
                column: "StatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ClockIns");

            migrationBuilder.DropTable(
                name: "EmployeeManager");

            migrationBuilder.DropTable(
                name: "Equipment");

            migrationBuilder.DropTable(
                name: "EvaluationForm");

            migrationBuilder.DropTable(
                name: "Request");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "EquipmentCategory");

            migrationBuilder.DropTable(
                name: "EvaluationType");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "RequestType");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropTable(
                name: "Team");
        }
    }
}
