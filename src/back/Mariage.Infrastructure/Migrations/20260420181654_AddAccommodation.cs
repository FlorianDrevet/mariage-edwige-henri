using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mariage.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAccommodation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accommodations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    UrlImage = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accommodations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccommodationAssignments",
                columns: table => new
                {
                    AccommodationAssignmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccommodationId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ResponseStatus = table.Column<string>(type: "text", nullable: false)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccommodationAssignments");

            migrationBuilder.DropTable(
                name: "Accommodations");
        }
    }
}
