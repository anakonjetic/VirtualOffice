using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtualOffice.Migrations
{
    public partial class EmployeeManagerUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.InsertData(
                table: "Employee",
                columns: new[] { "Id", "DateOfBirth", "EquipmentId", "FirstName", "LastName", "RemainingDaysOff", "SickLeaveDaysUsed", "TeamId", "UserId" },
                values: new object[,]
                {
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
                    { 14, new DateTime(2001, 5, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "4#5#6", "Lucija", "Kranjčec", 25, 1, 5, "f0e2bb7d-9ef4-421d-ad33-850f079c9507" },
                    { 15, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "1#2#3", "Luka", "Radošević", 12, 10, 4, "c74eba9f-b845-4a07-b524-16333e5d0a28" }
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.DeleteData(
                table: "EmployeeManager",
                keyColumns: new[] { "EmployeeId", "ManagerId" },
                keyValues: new object[] { 4, 1 });

            migrationBuilder.DeleteData(
                table: "EmployeeManager",
                keyColumns: new[] { "EmployeeId", "ManagerId" },
                keyValues: new object[] { 7, 1 });

            migrationBuilder.DeleteData(
                table: "EmployeeManager",
                keyColumns: new[] { "EmployeeId", "ManagerId" },
                keyValues: new object[] { 10, 1 });

            migrationBuilder.DeleteData(
                table: "EmployeeManager",
                keyColumns: new[] { "EmployeeId", "ManagerId" },
                keyValues: new object[] { 13, 1 });

            migrationBuilder.DeleteData(
                table: "EmployeeManager",
                keyColumns: new[] { "EmployeeId", "ManagerId" },
                keyValues: new object[] { 5, 2 });

            migrationBuilder.DeleteData(
                table: "EmployeeManager",
                keyColumns: new[] { "EmployeeId", "ManagerId" },
                keyValues: new object[] { 8, 2 });

            migrationBuilder.DeleteData(
                table: "EmployeeManager",
                keyColumns: new[] { "EmployeeId", "ManagerId" },
                keyValues: new object[] { 11, 2 });

            migrationBuilder.DeleteData(
                table: "EmployeeManager",
                keyColumns: new[] { "EmployeeId", "ManagerId" },
                keyValues: new object[] { 6, 3 });

            migrationBuilder.DeleteData(
                table: "EmployeeManager",
                keyColumns: new[] { "EmployeeId", "ManagerId" },
                keyValues: new object[] { 9, 3 });

            migrationBuilder.DeleteData(
                table: "EmployeeManager",
                keyColumns: new[] { "EmployeeId", "ManagerId" },
                keyValues: new object[] { 12, 3 });

            migrationBuilder.DeleteData(
                table: "EmployeeManager",
                keyColumns: new[] { "EmployeeId", "ManagerId" },
                keyValues: new object[] { 1, 14 });

            migrationBuilder.DeleteData(
                table: "EmployeeManager",
                keyColumns: new[] { "EmployeeId", "ManagerId" },
                keyValues: new object[] { 2, 14 });

            migrationBuilder.DeleteData(
                table: "EmployeeManager",
                keyColumns: new[] { "EmployeeId", "ManagerId" },
                keyValues: new object[] { 3, 14 });

            migrationBuilder.DeleteData(
                table: "EmployeeManager",
                keyColumns: new[] { "EmployeeId", "ManagerId" },
                keyValues: new object[] { 15, 14 });

            migrationBuilder.DeleteData(
                table: "Employee",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Employee",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Employee",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Employee",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Employee",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Employee",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Employee",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Employee",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Employee",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Employee",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Employee",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Employee",
                keyColumn: "Id",
                keyValue: 15);
        }
    }
}
