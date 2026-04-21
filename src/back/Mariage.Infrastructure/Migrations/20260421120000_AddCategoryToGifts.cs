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
            // The DB may have the old owned-type column "Category_Value" instead of "Category".
            // Rename it if it exists; otherwise add the column fresh.
            migrationBuilder.Sql("""
                IF COL_LENGTH('Gifts', 'Category_Value') IS NOT NULL
                BEGIN
                    EXEC sp_rename 'Gifts.Category_Value', 'Category', 'COLUMN';
                END
                ELSE IF COL_LENGTH('Gifts', 'Category') IS NULL
                BEGIN
                    ALTER TABLE [Gifts] ADD [Category] nvarchar(100) NOT NULL DEFAULT '';
                END
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Gifts",
                newName: "Category_Value");
        }
    }
}
