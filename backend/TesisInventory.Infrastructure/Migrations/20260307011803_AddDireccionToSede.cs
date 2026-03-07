using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TesisInventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDireccionToSede : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SedeIdSede",
                table: "Usuario",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Direccion",
                table: "Sede",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_SedeIdSede",
                table: "Usuario",
                column: "SedeIdSede");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuario_Sede_SedeIdSede",
                table: "Usuario",
                column: "SedeIdSede",
                principalTable: "Sede",
                principalColumn: "idSede");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuario_Sede_SedeIdSede",
                table: "Usuario");

            migrationBuilder.DropIndex(
                name: "IX_Usuario_SedeIdSede",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "SedeIdSede",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "Direccion",
                table: "Sede");
        }
    }
}
