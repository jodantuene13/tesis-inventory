using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TesisInventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactorAtributoUnidades : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Atributo_UnidadMedida_IdUnidadMedida",
                table: "Atributo");

            migrationBuilder.DropIndex(
                name: "IX_Atributo_IdUnidadMedida",
                table: "Atributo");

            migrationBuilder.DropColumn(
                name: "IdUnidadMedida",
                table: "Atributo");

            migrationBuilder.AddColumn<int>(
                name: "IdUnidadMedida",
                table: "ProductoAtributoValor",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AtributoUnidadMedida",
                columns: table => new
                {
                    IdAtributo = table.Column<int>(type: "int", nullable: false),
                    IdUnidadMedida = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AtributoUnidadMedida", x => new { x.IdAtributo, x.IdUnidadMedida });
                    table.ForeignKey(
                        name: "FK_AtributoUnidadMedida_Atributo_IdAtributo",
                        column: x => x.IdAtributo,
                        principalTable: "Atributo",
                        principalColumn: "IdAtributo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AtributoUnidadMedida_UnidadMedida_IdUnidadMedida",
                        column: x => x.IdUnidadMedida,
                        principalTable: "UnidadMedida",
                        principalColumn: "IdUnidadMedida",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ProductoAtributoValor_IdUnidadMedida",
                table: "ProductoAtributoValor",
                column: "IdUnidadMedida");

            migrationBuilder.CreateIndex(
                name: "IX_AtributoUnidadMedida_IdUnidadMedida",
                table: "AtributoUnidadMedida",
                column: "IdUnidadMedida");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductoAtributoValor_UnidadMedida_IdUnidadMedida",
                table: "ProductoAtributoValor",
                column: "IdUnidadMedida",
                principalTable: "UnidadMedida",
                principalColumn: "IdUnidadMedida",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductoAtributoValor_UnidadMedida_IdUnidadMedida",
                table: "ProductoAtributoValor");

            migrationBuilder.DropTable(
                name: "AtributoUnidadMedida");

            migrationBuilder.DropIndex(
                name: "IX_ProductoAtributoValor_IdUnidadMedida",
                table: "ProductoAtributoValor");

            migrationBuilder.DropColumn(
                name: "IdUnidadMedida",
                table: "ProductoAtributoValor");

            migrationBuilder.AddColumn<int>(
                name: "IdUnidadMedida",
                table: "Atributo",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Atributo_IdUnidadMedida",
                table: "Atributo",
                column: "IdUnidadMedida");

            migrationBuilder.AddForeignKey(
                name: "FK_Atributo_UnidadMedida_IdUnidadMedida",
                table: "Atributo",
                column: "IdUnidadMedida",
                principalTable: "UnidadMedida",
                principalColumn: "IdUnidadMedida",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
