using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class ExtendedUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "680742ad-1384-47eb-92ae-26c0050c3b3c",
                columns: new[] { "ConcurrencyStamp", "DateOfBirth", "FirstName", "LastName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f50e7405-eccd-4218-9cb3-aebe8c171ad2", new DateOnly(1950, 12, 1), "Default", "Admin", "AQAAAAIAAYagAAAAELIXhFCslqMmePDUJqlgWvsvCvZIPZ8Re4DZ1CLEUO+YZ7fXqsDhTNvrtZOG0QgCEQ==", "04e2c93e-da86-415c-905f-b9a8ae9e466e" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "680742ad-1384-47eb-92ae-26c0050c3b3c",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "28eeab1e-5b34-4548-98d4-eaecc4e4584d", "AQAAAAIAAYagAAAAEKgzSGHJGrngZpqxyikuBSGpl+7qNR5RyEyulg1qdEvvcDYhnPKiHqy59C8x0/2V+Q==", "58a324d6-b5a6-4a4f-aa07-53f6dfb4b077" });
        }
    }
}
