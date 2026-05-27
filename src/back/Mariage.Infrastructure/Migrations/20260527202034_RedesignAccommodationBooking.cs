using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mariage.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RedesignAccommodationBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccommodationAssignments");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Accommodations",
                newName: "PricePerPersonPerNight");

            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "Accommodations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfNights",
                table: "Accommodations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AccommodationBookings",
                columns: table => new
                {
                    AccommodationBookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccommodationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NumberOfPersons = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    BookingStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookerFirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookerLastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookerEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccommodationBookings", x => new { x.AccommodationBookingId, x.AccommodationId });
                    table.ForeignKey(
                        name: "FK_AccommodationBookings_Accommodations_AccommodationId",
                        column: x => x.AccommodationId,
                        principalTable: "Accommodations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccommodationBookings_AccommodationId",
                table: "AccommodationBookings",
                column: "AccommodationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccommodationBookings");

            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Accommodations");

            migrationBuilder.DropColumn(
                name: "NumberOfNights",
                table: "Accommodations");

            migrationBuilder.RenameColumn(
                name: "PricePerPersonPerNight",
                table: "Accommodations",
                newName: "Price");

            migrationBuilder.CreateTable(
                name: "AccommodationAssignments",
                columns: table => new
                {
                    AccommodationAssignmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccommodationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResponseStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccommodationAssignments", x => new { x.AccommodationAssignmentId, x.AccommodationId });
                    table.ForeignKey(
                        name: "FK_AccommodationAssignments_Accommodations_AccommodationId",
                        column: x => x.AccommodationId,
                        principalTable: "Accommodations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccommodationAssignments_AccommodationId",
                table: "AccommodationAssignments",
                column: "AccommodationId");
        }
    }
}
