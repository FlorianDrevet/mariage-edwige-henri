using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mariage.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddHasLoggedInToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasLoggedIn",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasLoggedIn",
                table: "Users");
        }
    }
}
