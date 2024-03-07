using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtualOffice.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Request_Employee_EmployeeId",
                table: "Request");

            migrationBuilder.DropForeignKey(
                name: "FK_Request_Employee_ManagerId",
                table: "Request");

            migrationBuilder.AddForeignKey(
                name: "FK_Request_Employee_EmployeeId",
                table: "Request",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Request_Employee_ManagerId",
                table: "Request",
                column: "ManagerId",
                principalTable: "Employee",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Request_Employee_EmployeeId",
                table: "Request");

            migrationBuilder.DropForeignKey(
                name: "FK_Request_Employee_ManagerId",
                table: "Request");

            migrationBuilder.AddForeignKey(
                name: "FK_Request_Employee_EmployeeId",
                table: "Request",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Request_Employee_ManagerId",
                table: "Request",
                column: "ManagerId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
