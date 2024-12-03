using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace biblioteca_backend.Migrations
{
    /// <inheritdoc />
    public partial class v103 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Teleforno",
                table: "Perfil",
                newName: "Telefono");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Telefono",
                table: "Perfil",
                newName: "Teleforno");
        }
    }
}
