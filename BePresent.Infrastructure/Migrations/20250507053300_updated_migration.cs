using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BePresent.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updated_migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reason",
                table: "Attendance");

            migrationBuilder.AddColumn<string>(
                name: "CheckInReason",
                table: "Attendance",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CheckOutReason",
                table: "Attendance",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckInReason",
                table: "Attendance");

            migrationBuilder.DropColumn(
                name: "CheckOutReason",
                table: "Attendance");

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "Attendance",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
