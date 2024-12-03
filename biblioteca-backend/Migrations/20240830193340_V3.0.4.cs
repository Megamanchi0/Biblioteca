using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace biblioteca_backend.Migrations
{
    /// <inheritdoc />
    public partial class V304 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Stock",
                table: "Libro");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "Libro",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
