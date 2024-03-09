using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtualOffice.Data.Migrations
{
    public partial class DataSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                table: "User",
                columns: new[] { "Id", "Email", "Password", "Username" },
                values: new object[,]
                {
                    { 1, "user1@example.com", "password1", "user1" },
                    { 2, "user2@example.com", "password2", "user2" }
                });

            migrationBuilder.InsertData(
                table: "Employee",
                columns: new[] { "Id", "DateOfBirth", "EquipmentId", "FirstName", "LastName", "RemainingDaysOff", "SickLeaveDaysUsed", "TeamId", "UserId" },
                values: new object[] { 1, new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "1#2#3", "John", "Doe", 20, 5, 1, 1 });

            migrationBuilder.InsertData(
                table: "Employee",
                columns: new[] { "Id", "DateOfBirth", "EquipmentId", "FirstName", "LastName", "RemainingDaysOff", "SickLeaveDaysUsed", "TeamId", "UserId" },
                values: new object[] { 2, new DateTime(1995, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "4#5#6", "Jane", "Doe", 15, 2, 2, 2 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employee",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Employee",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Equipment",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Equipment",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Equipment",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Equipment",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Equipment",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Equipment",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "EquipmentCategory",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "EquipmentCategory",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "EquipmentCategory",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "EquipmentCategory",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "EquipmentCategory",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Team",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Team",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Team",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Team",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Team",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
