using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtualOffice.Migrations
{
    public partial class UserIdUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "Id",
                keyValue: 1,
                column: "UserId",
                value: "akonjetic@tvz.hr");

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "Id",
                keyValue: 2,
                column: "UserId",
                value: "mtkalec@tvz.hr");

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "Id",
                keyValue: 3,
                column: "UserId",
                value: "ijelinic@tvz.hr");

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "Id",
                keyValue: 14,
                column: "UserId",
                value: "lkranjcec@tvz.hr");

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "Id",
                keyValue: 15,
                column: "UserId",
                value: "lradosev1@tvz.hr");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "Id",
                keyValue: 14,
                column: "UserId",
                value: "f0e2bb7d-9ef4-421d-ad33-850f079c9507");

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "Id",
                keyValue: 15,
                column: "UserId",
                value: "c74eba9f-b845-4a07-b524-16333e5d0a28");
        }
    }
}
