using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace biblioteca_backend.Migrations
{
    /// <inheritdoc />
    public partial class v200 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rol",
                table: "Perfil");

            migrationBuilder.AddColumn<int>(
                name: "IdRol",
                table: "Perfil",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Rol",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rol", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Perfil_IdRol",
                table: "Perfil",
                column: "IdRol");

            migrationBuilder.AddForeignKey(
                name: "FK_Perfil_Rol_IdRol",
                table: "Perfil",
                column: "IdRol",
                principalTable: "Rol",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Perfil_Rol_IdRol",
                table: "Perfil");

            migrationBuilder.DropTable(
                name: "Rol");

            migrationBuilder.DropIndex(
                name: "IX_Perfil_IdRol",
                table: "Perfil");

            migrationBuilder.DropColumn(
                name: "IdRol",
                table: "Perfil");

            migrationBuilder.AddColumn<string>(
                name: "Rol",
                table: "Perfil",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
