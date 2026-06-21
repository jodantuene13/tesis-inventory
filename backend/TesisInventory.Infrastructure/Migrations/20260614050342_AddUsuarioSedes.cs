using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TesisInventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUsuarioSedes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "limitarOperacionSedePrimaria",
                table: "Usuario",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "todasLasSedes",
                table: "Usuario",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "UsuarioSede",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    IdSede = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioSede", x => new { x.IdUsuario, x.IdSede });
                    table.ForeignKey(
                        name: "FK_UsuarioSede_Sede_IdSede",
                        column: x => x.IdSede,
                        principalTable: "Sede",
                        principalColumn: "idSede",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioSede_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "idUsuario",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioSede_IdSede",
                table: "UsuarioSede",
                column: "IdSede");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsuarioSede");

            migrationBuilder.DropColumn(
                name: "limitarOperacionSedePrimaria",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "todasLasSedes",
                table: "Usuario");
        }
    }
}
