using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class test173 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "680742ad-1384-47eb-92ae-26c0050c3b3c",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1a48eb51-7dd2-46b2-83d0-913794c4c232", "AQAAAAIAAYagAAAAEAsiPVQvGyfKNJkRrQ6Ad7upDnhy3sfK6QH1qt7fvjtkhAARYOb7rjcP5jknAoxZ7A==", "346f87cb-1adc-4146-bc38-210207ff64cd" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "680742ad-1384-47eb-92ae-26c0050c3b3c",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0f9517e6-c27d-43e1-a313-2f1f56bca29f", "AQAAAAIAAYagAAAAEL8B5tFIybo1A7/qzoeF0BfC6i3sN3KFCIDROe7F3cFZ98CbhQWZ5lnKlKNWx/NK3Q==", "960eaa50-a2ee-4cb2-a95a-3ecd139b5da9" });
        }
    }
}
