using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TesisInventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUnidadMedida : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Unidad",
                table: "Atributo");

            migrationBuilder.AddColumn<int>(
                name: "IdUnidadMedida",
                table: "GrupoAtributoItem",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdUnidadMedida",
                table: "Atributo",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UnidadMedida",
                columns: table => new
                {
                    IdUnidadMedida = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Simbolo = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Activo = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnidadMedida", x => x.IdUnidadMedida);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_GrupoAtributoItem_IdUnidadMedida",
                table: "GrupoAtributoItem",
                column: "IdUnidadMedida");

            migrationBuilder.CreateIndex(
                name: "IX_Atributo_IdUnidadMedida",
                table: "Atributo",
                column: "IdUnidadMedida");

            migrationBuilder.CreateIndex(
                name: "IX_UnidadMedida_Simbolo",
                table: "UnidadMedida",
                column: "Simbolo",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Atributo_UnidadMedida_IdUnidadMedida",
                table: "Atributo",
                column: "IdUnidadMedida",
                principalTable: "UnidadMedida",
                principalColumn: "IdUnidadMedida",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_GrupoAtributoItem_UnidadMedida_IdUnidadMedida",
                table: "GrupoAtributoItem",
                column: "IdUnidadMedida",
                principalTable: "UnidadMedida",
                principalColumn: "IdUnidadMedida",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Atributo_UnidadMedida_IdUnidadMedida",
                table: "Atributo");

            migrationBuilder.DropForeignKey(
                name: "FK_GrupoAtributoItem_UnidadMedida_IdUnidadMedida",
                table: "GrupoAtributoItem");

            migrationBuilder.DropTable(
                name: "UnidadMedida");

            migrationBuilder.DropIndex(
                name: "IX_GrupoAtributoItem_IdUnidadMedida",
                table: "GrupoAtributoItem");

            migrationBuilder.DropIndex(
                name: "IX_Atributo_IdUnidadMedida",
                table: "Atributo");

            migrationBuilder.DropColumn(
                name: "IdUnidadMedida",
                table: "GrupoAtributoItem");

            migrationBuilder.DropColumn(
                name: "IdUnidadMedida",
                table: "Atributo");

            migrationBuilder.AddColumn<string>(
                name: "Unidad",
                table: "Atributo",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
