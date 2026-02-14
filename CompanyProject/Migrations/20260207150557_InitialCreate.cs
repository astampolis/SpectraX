using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyProject.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IoTDevices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FirmwareVersion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastSeen = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IoTDevices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EnergyAlerts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IoTDeviceId = table.Column<int>(type: "int", nullable: false),
                    Severity = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TriggeredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Acknowledged = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnergyAlerts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnergyAlerts_IoTDevices_IoTDeviceId",
                        column: x => x.IoTDeviceId,
                        principalTable: "IoTDevices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EnergyReadings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IoTDeviceId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PowerWatts = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Voltage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrentAmps = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EnergyKwh = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnergyReadings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnergyReadings_IoTDevices_IoTDeviceId",
                        column: x => x.IoTDeviceId,
                        principalTable: "IoTDevices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.InsertData(
                table: "IoTDevices",
                columns: new[] { "Id", "FirmwareVersion", "LastSeen", "Location", "Name", "Status" },
                values: new object[,]
                {
                    { 1, "1.4.2", new DateTime(2023, 1, 25, 9, 30, 0, 0, DateTimeKind.Unspecified), "HQ - Floor 1", "Main Floor Meter", "Online" },
                    { 2, "1.2.9", new DateTime(2023, 1, 25, 8, 45, 0, 0, DateTimeKind.Unspecified), "Research Lab", "Lab HVAC Monitor", "Maintenance" }
                });

            migrationBuilder.InsertData(
                table: "EnergyAlerts",
                columns: new[] { "Id", "Acknowledged", "IoTDeviceId", "Message", "Severity", "TriggeredAt" },
                values: new object[,]
                {
                    { 1, false, 2, "HVAC consumption exceeded threshold.", "High", new DateTime(2023, 1, 25, 8, 40, 0, 0, DateTimeKind.Unspecified) },
                    { 2, true, 1, "Power factor drifting out of range.", "Medium", new DateTime(2023, 1, 25, 9, 10, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "EnergyReadings",
                columns: new[] { "Id", "CurrentAmps", "EnergyKwh", "IoTDeviceId", "PowerWatts", "Timestamp", "Voltage" },
                values: new object[,]
                {
                    { 1, 18.3m, 4.2m, 1, 4200m, new DateTime(2023, 1, 25, 9, 0, 0, 0, DateTimeKind.Unspecified), 230m },
                    { 2, 13.9m, 3.1m, 2, 3200m, new DateTime(2023, 1, 25, 8, 30, 0, 0, DateTimeKind.Unspecified), 230m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_EnergyAlerts_IoTDeviceId",
                table: "EnergyAlerts",
                column: "IoTDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_EnergyReadings_IoTDeviceId",
                table: "EnergyReadings",
                column: "IoTDeviceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnergyAlerts");

            migrationBuilder.DropTable(
                name: "EnergyReadings");

            migrationBuilder.DropTable(
                name: "IoTDevices");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "33244a2a-62a8-4f91-83ac-6435a1348629",
                column: "ConcurrencyStamp",
                value: "8331ce7f-078b-4a7f-8889-c93e8bfbe2e0");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "40c6bc97-f08f-41e4-bf60-ccd30ff4ab41",
                column: "ConcurrencyStamp",
                value: "a6cbff0d-d2a9-451b-b34f-fccc7ec91498");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0463448f-fe47-41ab-9e99-b0245c4e7e84",
                columns: new[] { "PasswordHash", "SecurityStamp" },
                values: new object[] { "AG0jCwUCPOIEMvQ2xLJ4W3/Lc5w/6KLIrtIGvA/m7nm5uUd6K6cnVRTzdjvo/3fwmA==", "92cfbf6a-32e3-4f93-aca7-f582475dc8ea" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4808c606-89cf-4a92-8ff6-33074a34a335",
                columns: new[] { "PasswordHash", "SecurityStamp" },
                values: new object[] { "ACmkVSva7BwlaZOTzWRn7ntlXlrydK3fLdIRKtPjEKEszHpnHMVaPzn1XJqWZSQWNQ==", "0258cbb5-ddac-4430-b9a5-10f356bd9bea" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ed3f4a9b-6a9d-4514-9197-64c599ca7cde",
                columns: new[] { "PasswordHash", "SecurityStamp" },
                values: new object[] { "AJpxvvQYSSzYFodoL5i1urNx158FRKpt2gB2xzRsMJnSU3nudCoGuuvEYDq8vrpLkQ==", "a766d1c6-ebd7-4689-bde5-e3d5aeb6fde5" });
        }
    }
}
