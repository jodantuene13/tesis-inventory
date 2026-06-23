using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TesisInventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGruposAtributos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GrupoAtributo",
                columns: table => new
                {
                    IdGrupoAtributo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CodigoGrupo = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Separador = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UnidadSufijo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Activo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrupoAtributo", x => x.IdGrupoAtributo);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FamiliaGrupoAtributo",
                columns: table => new
                {
                    IdFamiliaGrupoAtributo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdFamilia = table.Column<int>(type: "int", nullable: false),
                    IdGrupoAtributo = table.Column<int>(type: "int", nullable: false),
                    Obligatorio = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Activo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamiliaGrupoAtributo", x => x.IdFamiliaGrupoAtributo);
                    table.ForeignKey(
                        name: "FK_FamiliaGrupoAtributo_Familia_IdFamilia",
                        column: x => x.IdFamilia,
                        principalTable: "Familia",
                        principalColumn: "IdFamilia",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FamiliaGrupoAtributo_GrupoAtributo_IdGrupoAtributo",
                        column: x => x.IdGrupoAtributo,
                        principalTable: "GrupoAtributo",
                        principalColumn: "IdGrupoAtributo",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GrupoAtributoItem",
                columns: table => new
                {
                    IdGrupoAtributoItem = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdGrupoAtributo = table.Column<int>(type: "int", nullable: false),
                    IdAtributo = table.Column<int>(type: "int", nullable: false),
                    Orden = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrupoAtributoItem", x => x.IdGrupoAtributoItem);
                    table.ForeignKey(
                        name: "FK_GrupoAtributoItem_Atributo_IdAtributo",
                        column: x => x.IdAtributo,
                        principalTable: "Atributo",
                        principalColumn: "IdAtributo",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GrupoAtributoItem_GrupoAtributo_IdGrupoAtributo",
                        column: x => x.IdGrupoAtributo,
                        principalTable: "GrupoAtributo",
                        principalColumn: "IdGrupoAtributo",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_FamiliaGrupoAtributo_IdFamilia_IdGrupoAtributo",
                table: "FamiliaGrupoAtributo",
                columns: new[] { "IdFamilia", "IdGrupoAtributo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FamiliaGrupoAtributo_IdGrupoAtributo",
                table: "FamiliaGrupoAtributo",
                column: "IdGrupoAtributo");

            migrationBuilder.CreateIndex(
                name: "IX_GrupoAtributo_CodigoGrupo",
                table: "GrupoAtributo",
                column: "CodigoGrupo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GrupoAtributoItem_IdAtributo",
                table: "GrupoAtributoItem",
                column: "IdAtributo");

            migrationBuilder.CreateIndex(
                name: "IX_GrupoAtributoItem_IdGrupoAtributo_IdAtributo",
                table: "GrupoAtributoItem",
                columns: new[] { "IdGrupoAtributo", "IdAtributo" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FamiliaGrupoAtributo");

            migrationBuilder.DropTable(
                name: "GrupoAtributoItem");

            migrationBuilder.DropTable(
                name: "GrupoAtributo");
        }
    }
}
