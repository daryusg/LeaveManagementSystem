using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LeaveManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedingDefaultRolesAndUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3f6ecb32-cc6e-4eb6-a90e-7268e00edeb3", null, "Supervisor", "SUPERVISOR" },
                    { "ecb2c98b-8f29-41a2-b892-75ebeb75ccd0", null, "Employee", "EMPLOYEE" },
                    { "f7e413fe-dbb4-4a48-882f-679aae30b6d7", null, "Administrator", "ADMINISTRATOR" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "680742ad-1384-47eb-92ae-26c0050c3b3c", 0, "28eeab1e-5b34-4548-98d4-eaecc4e4584d", "daryus@europe.com", true, false, null, "DARYUS@EUROPE.COM", null, "AQAAAAIAAYagAAAAEKgzSGHJGrngZpqxyikuBSGpl+7qNR5RyEyulg1qdEvvcDYhnPKiHqy59C8x0/2V+Q==", null, false, "58a324d6-b5a6-4a4f-aa07-53f6dfb4b077", false, "daryus@europe.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "f7e413fe-dbb4-4a48-882f-679aae30b6d7", "680742ad-1384-47eb-92ae-26c0050c3b3c" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3f6ecb32-cc6e-4eb6-a90e-7268e00edeb3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ecb2c98b-8f29-41a2-b892-75ebeb75ccd0");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "f7e413fe-dbb4-4a48-882f-679aae30b6d7", "680742ad-1384-47eb-92ae-26c0050c3b3c" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f7e413fe-dbb4-4a48-882f-679aae30b6d7");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "680742ad-1384-47eb-92ae-26c0050c3b3c");
        }
    }
}
