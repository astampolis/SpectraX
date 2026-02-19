using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyProject.Migrations
{
    [DbContext(typeof(CompanyProject.Data.ApplicationDbContext))]
    [Migration("20260219180000_AddDepartmentAndScheduleIndex")]
    public class AddDepartmentAndScheduleIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTasks_EmployeeId_TaskStart_TaskEnd",
                table: "EmployeeTasks",
                columns: new[] { "EmployeeId", "TaskStart", "TaskEnd" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EmployeeTasks_EmployeeId_TaskStart_TaskEnd",
                table: "EmployeeTasks");

            migrationBuilder.DropColumn(
                name: "Department",
                table: "AspNetUsers");
        }
    }
}
