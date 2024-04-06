using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wedding_Planner_System.Migrations
{
    /// <inheritdoc />
    public partial class ReservationEdit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Service",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Service",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Reservations");
        }
    }
}
