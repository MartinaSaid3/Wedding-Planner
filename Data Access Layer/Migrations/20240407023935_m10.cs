using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wedding_Planner_System.Migrations
{
    /// <inheritdoc />
    public partial class m10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rates_Reservations_ReservationId",
                table: "Rates");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Rates");

            migrationBuilder.DropColumn(
                name: "ReviewDate",
                table: "Rates");

            migrationBuilder.RenameColumn(
                name: "ReservationId",
                table: "Rates",
                newName: "VenueId");

            migrationBuilder.RenameIndex(
                name: "IX_Rates_ReservationId",
                table: "Rates",
                newName: "IX_Rates_VenueId");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Rates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Rates_Venues_VenueId",
                table: "Rates",
                column: "VenueId",
                principalTable: "Venues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rates_Venues_VenueId",
                table: "Rates");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Rates");

            migrationBuilder.RenameColumn(
                name: "VenueId",
                table: "Rates",
                newName: "ReservationId");

            migrationBuilder.RenameIndex(
                name: "IX_Rates_VenueId",
                table: "Rates",
                newName: "IX_Rates_ReservationId");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Rates",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReviewDate",
                table: "Rates",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Rates_Reservations_ReservationId",
                table: "Rates",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
