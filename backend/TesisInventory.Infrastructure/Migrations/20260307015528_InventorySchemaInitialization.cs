using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TesisInventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InventorySchemaInitialization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Sede",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CodigoSede",
                table: "Sede",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Atributo",
                columns: table => new
                {
                    IdAtributo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CodigoAtributo = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TipoDato = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Unidad = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Activo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Atributo", x => x.IdAtributo);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Rubro",
                columns: table => new
                {
                    IdRubro = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CodigoRubro = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Activo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rubro", x => x.IdRubro);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AtributoOpcion",
                columns: table => new
                {
                    IdAtributoOpcion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdAtributo = table.Column<int>(type: "int", nullable: false),
                    CodigoOpcion = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Valor = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Orden = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AtributoOpcion", x => x.IdAtributoOpcion);
                    table.ForeignKey(
                        name: "FK_AtributoOpcion_Atributo_IdAtributo",
                        column: x => x.IdAtributo,
                        principalTable: "Atributo",
                        principalColumn: "IdAtributo",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Familia",
                columns: table => new
                {
                    IdFamilia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdRubro = table.Column<int>(type: "int", nullable: false),
                    CodigoFamilia = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Activo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Familia", x => x.IdFamilia);
                    table.ForeignKey(
                        name: "FK_Familia_Rubro_IdRubro",
                        column: x => x.IdRubro,
                        principalTable: "Rubro",
                        principalColumn: "IdRubro",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FamiliaAtributo",
                columns: table => new
                {
                    IdFamiliaAtributo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdFamilia = table.Column<int>(type: "int", nullable: false),
                    IdAtributo = table.Column<int>(type: "int", nullable: false),
                    Obligatorio = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Orden = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamiliaAtributo", x => x.IdFamiliaAtributo);
                    table.ForeignKey(
                        name: "FK_FamiliaAtributo_Atributo_IdAtributo",
                        column: x => x.IdAtributo,
                        principalTable: "Atributo",
                        principalColumn: "IdAtributo",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FamiliaAtributo_Familia_IdFamilia",
                        column: x => x.IdFamilia,
                        principalTable: "Familia",
                        principalColumn: "IdFamilia",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Producto",
                columns: table => new
                {
                    IdProducto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdFamilia = table.Column<int>(type: "int", nullable: false),
                    Sku = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UnidadMedida = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Activo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producto", x => x.IdProducto);
                    table.ForeignKey(
                        name: "FK_Producto_Familia_IdFamilia",
                        column: x => x.IdFamilia,
                        principalTable: "Familia",
                        principalColumn: "IdFamilia",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProductoAtributoValor",
                columns: table => new
                {
                    IdProductoAtributoValor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdProducto = table.Column<int>(type: "int", nullable: false),
                    IdAtributo = table.Column<int>(type: "int", nullable: false),
                    ValorTexto = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ValorNumero = table.Column<int>(type: "int", nullable: true),
                    ValorDecimal = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    ValorBool = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    ValorLista = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductoAtributoValor", x => x.IdProductoAtributoValor);
                    table.ForeignKey(
                        name: "FK_ProductoAtributoValor_Atributo_IdAtributo",
                        column: x => x.IdAtributo,
                        principalTable: "Atributo",
                        principalColumn: "IdAtributo",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductoAtributoValor_Producto_IdProducto",
                        column: x => x.IdProducto,
                        principalTable: "Producto",
                        principalColumn: "IdProducto",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Stock",
                columns: table => new
                {
                    IdStock = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdProducto = table.Column<int>(type: "int", nullable: false),
                    IdSede = table.Column<int>(type: "int", nullable: false),
                    CantidadActual = table.Column<int>(type: "int", nullable: false),
                    PuntoReposicion = table.Column<int>(type: "int", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stock", x => x.IdStock);
                    table.ForeignKey(
                        name: "FK_Stock_Producto_IdProducto",
                        column: x => x.IdProducto,
                        principalTable: "Producto",
                        principalColumn: "IdProducto",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Stock_Sede_IdSede",
                        column: x => x.IdSede,
                        principalTable: "Sede",
                        principalColumn: "idSede",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Atributo_CodigoAtributo",
                table: "Atributo",
                column: "CodigoAtributo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AtributoOpcion_IdAtributo",
                table: "AtributoOpcion",
                column: "IdAtributo");

            migrationBuilder.CreateIndex(
                name: "IX_Familia_IdRubro_CodigoFamilia",
                table: "Familia",
                columns: new[] { "IdRubro", "CodigoFamilia" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FamiliaAtributo_IdAtributo",
                table: "FamiliaAtributo",
                column: "IdAtributo");

            migrationBuilder.CreateIndex(
                name: "IX_FamiliaAtributo_IdFamilia_IdAtributo",
                table: "FamiliaAtributo",
                columns: new[] { "IdFamilia", "IdAtributo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Producto_IdFamilia",
                table: "Producto",
                column: "IdFamilia");

            migrationBuilder.CreateIndex(
                name: "IX_Producto_Sku",
                table: "Producto",
                column: "Sku",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductoAtributoValor_IdAtributo",
                table: "ProductoAtributoValor",
                column: "IdAtributo");

            migrationBuilder.CreateIndex(
                name: "IX_ProductoAtributoValor_IdProducto_IdAtributo",
                table: "ProductoAtributoValor",
                columns: new[] { "IdProducto", "IdAtributo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rubro_CodigoRubro",
                table: "Rubro",
                column: "CodigoRubro",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stock_IdProducto_IdSede",
                table: "Stock",
                columns: new[] { "IdProducto", "IdSede" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stock_IdSede",
                table: "Stock",
                column: "IdSede");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AtributoOpcion");

            migrationBuilder.DropTable(
                name: "FamiliaAtributo");

            migrationBuilder.DropTable(
                name: "ProductoAtributoValor");

            migrationBuilder.DropTable(
                name: "Stock");

            migrationBuilder.DropTable(
                name: "Atributo");

            migrationBuilder.DropTable(
                name: "Producto");

            migrationBuilder.DropTable(
                name: "Familia");

            migrationBuilder.DropTable(
                name: "Rubro");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Sede");

            migrationBuilder.DropColumn(
                name: "CodigoSede",
                table: "Sede");

            migrationBuilder.AddColumn<int>(
                name: "SedeIdSede",
                table: "Usuario",
                type: "int",
                nullable: true);

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
    }
}
