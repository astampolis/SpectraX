using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyProject.Migrations
{
    public partial class migrationname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApprovalStatus",
                table: "EmployeeLeave",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DecisionDateUtc",
                table: "EmployeeLeave",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LeaveTypeId",
                table: "EmployeeLeave",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ManagerId",
                table: "EmployeeLeave",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "InAppNotifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InAppNotifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InAppNotifications_AspNetUsers_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LeaveApprovalComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeLeaveId = table.Column<int>(type: "int", nullable: false),
                    AuthorId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveApprovalComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeaveApprovalComments_EmployeeLeave_EmployeeLeaveId",
                        column: x => x.EmployeeLeaveId,
                        principalTable: "EmployeeLeave",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LeaveTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    MonthlyAccrualDays = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    YearlyAccrualDays = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CarryOverLimitDays = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimesheetWeeks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WeekStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalHours = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SubmissionComment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ManagerComment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimesheetWeeks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimesheetWeeks_AspNetUsers_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "33244a2a-62a8-4f91-83ac-6435a1348629",
                column: "ConcurrencyStamp",
                value: "068b6598-965c-47b4-ada4-58eca2e0e28f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "40c6bc97-f08f-41e4-bf60-ccd30ff4ab41",
                column: "ConcurrencyStamp",
                value: "b502bd66-7d09-4bce-8bf1-70666acdd4c1");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0463448f-fe47-41ab-9e99-b0245c4e7e84",
                columns: new[] { "PasswordHash", "SecurityStamp" },
                values: new object[] { "AAPh7biVjeWCSeUbn3uI2cOq0mnJromaUIt2OD7kedUL7aoZYXXyvPu8SYDriTXktw==", "402c0c7e-45d7-4dd1-9d28-f02bfd36ebdb" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4808c606-89cf-4a92-8ff6-33074a34a335",
                columns: new[] { "PasswordHash", "SecurityStamp" },
                values: new object[] { "AFPtWy+qlfMLcSPPsPVDEyEtTXCF+127UfydPckdVtVuRszKyw8VNlSwEG2HOIzxPg==", "850db7c2-9181-4bb9-8f3d-6feacfab7e97" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ed3f4a9b-6a9d-4514-9197-64c599ca7cde",
                columns: new[] { "PasswordHash", "SecurityStamp" },
                values: new object[] { "AMzDJnJ+bczWSLp+UoX2GvibI4k3zIoATnbV8AWc6Nh8pXoucYSZdZaZwVLsUkcHQA==", "a8b72274-8a78-49f5-8cb8-4e9c2b4e1e91" });

            migrationBuilder.UpdateData(
                table: "EmployeeLeave",
                keyColumn: "Id",
                keyValue: 1,
                column: "ApprovalStatus",
                value: "Pending");

            migrationBuilder.UpdateData(
                table: "EmployeeLeave",
                keyColumn: "Id",
                keyValue: 3,
                column: "ApprovalStatus",
                value: "Pending");

            migrationBuilder.UpdateData(
                table: "EmployeeLeave",
                keyColumn: "Id",
                keyValue: 4,
                column: "ApprovalStatus",
                value: "Pending");

            migrationBuilder.InsertData(
                table: "LeaveTypes",
                columns: new[] { "Id", "CarryOverLimitDays", "IsPaid", "MonthlyAccrualDays", "Name", "YearlyAccrualDays" },
                values: new object[,]
                {
                    { 1, 5m, true, 1.5m, "Paid Leave", 18m },
                    { 2, 0m, true, 0.8m, "Sick Leave", 10m },
                    { 3, 0m, false, 0m, "Unpaid Leave", 0m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeLeave_LeaveTypeId",
                table: "EmployeeLeave",
                column: "LeaveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_InAppNotifications_EmployeeId",
                table: "InAppNotifications",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveApprovalComments_EmployeeLeaveId",
                table: "LeaveApprovalComments",
                column: "EmployeeLeaveId");

            migrationBuilder.CreateIndex(
                name: "IX_TimesheetWeeks_EmployeeId",
                table: "TimesheetWeeks",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeLeave_LeaveTypes_LeaveTypeId",
                table: "EmployeeLeave",
                column: "LeaveTypeId",
                principalTable: "LeaveTypes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeLeave_LeaveTypes_LeaveTypeId",
                table: "EmployeeLeave");

            migrationBuilder.DropTable(
                name: "InAppNotifications");

            migrationBuilder.DropTable(
                name: "LeaveApprovalComments");

            migrationBuilder.DropTable(
                name: "LeaveTypes");

            migrationBuilder.DropTable(
                name: "TimesheetWeeks");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeLeave_LeaveTypeId",
                table: "EmployeeLeave");

            migrationBuilder.DropColumn(
                name: "ApprovalStatus",
                table: "EmployeeLeave");

            migrationBuilder.DropColumn(
                name: "DecisionDateUtc",
                table: "EmployeeLeave");

            migrationBuilder.DropColumn(
                name: "LeaveTypeId",
                table: "EmployeeLeave");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "EmployeeLeave");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "33244a2a-62a8-4f91-83ac-6435a1348629",
                column: "ConcurrencyStamp",
                value: "de3e0c07-1eec-4b62-a9d0-250b0a7f5695");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "40c6bc97-f08f-41e4-bf60-ccd30ff4ab41",
                column: "ConcurrencyStamp",
                value: "d666bf41-4da7-44fa-985d-ff54b0727585");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0463448f-fe47-41ab-9e99-b0245c4e7e84",
                columns: new[] { "PasswordHash", "SecurityStamp" },
                values: new object[] { "AFdoq7r3W0/n4BWkEnNnfmP0KvVPxPqTa+0P1wR8jLD1UaB3bNO1+TxqhbanFsfzRw==", "e38eccbe-6bd2-47d3-9071-c6edb8b556f3" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4808c606-89cf-4a92-8ff6-33074a34a335",
                columns: new[] { "PasswordHash", "SecurityStamp" },
                values: new object[] { "AKbHTLLOeUi092mWIJEGlXaTSNW2D51aww2KbXHkl2bia1US19twlIXXZ3gv2S1jdA==", "bccb49b3-4355-41fc-9b4a-6deab244134c" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ed3f4a9b-6a9d-4514-9197-64c599ca7cde",
                columns: new[] { "PasswordHash", "SecurityStamp" },
                values: new object[] { "AI4++yl4Y+G6v5tjDXOfh0mIA5cygU4U7AI+qPcmf9hUy6k86/EWTobA31cEgKXPWw==", "7b966a54-eea1-494e-b4b2-24e667d94a84" });
        }
    }
}
