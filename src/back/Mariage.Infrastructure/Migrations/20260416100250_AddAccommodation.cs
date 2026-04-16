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
            migrationBuilder.AddColumn<Guid>(
                name: "AccommodationId",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAccommodationAccepted",
                table: "Users",
                type: "boolean",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Accommodations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    UrlImage = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accommodations", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accommodations");

            migrationBuilder.DropColumn(
                name: "AccommodationId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsAccommodationAccepted",
                table: "Users");
        }
    }
}
