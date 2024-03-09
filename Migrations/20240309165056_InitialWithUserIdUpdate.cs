using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtualOffice.Migrations
{
    public partial class InitialWithUserIdUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "Id",
                keyValue: 1,
                column: "UserId",
                value: "1e95f075-9cbd-4252-8a25-faeb03e0449e");

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "Id",
                keyValue: 2,
                column: "UserId",
                value: "9ad46335-32b4-492d-8592-4379e0f2f108");

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "Id",
                keyValue: 3,
                column: "UserId",
                value: "f0e2bb7d-9ef4-421d-ad33-850f079c9507");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "Id",
                keyValue: 1,
                column: "UserId",
                value: "9466791b-d6f5-4aa3-8ae7-b4372e003c59");

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "Id",
                keyValue: 2,
                column: "UserId",
                value: "a3128091-0f41-4abe-a532-5cc1fdb40c0a");

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "Id",
                keyValue: 3,
                column: "UserId",
                value: "aa004dd5-c419-449e-a41a-abf5e5d3c1f1");
        }
    }
}
