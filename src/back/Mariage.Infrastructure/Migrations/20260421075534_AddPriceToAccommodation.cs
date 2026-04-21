using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mariage.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPriceToAccommodation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Accommodations",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Accommodations");
        }
    }
}
