using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mariage.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryToGifts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Gifts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Gifts");
        }
    }
}
