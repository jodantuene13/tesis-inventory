using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TesisInventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAlertaStockTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AlertaStock",
                columns: table => new
                {
                    idAlertaStock = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    idProducto = table.Column<int>(type: "int", nullable: false),
                    idSede = table.Column<int>(type: "int", nullable: false),
                    stockAlMomento = table.Column<int>(type: "int", nullable: false),
                    puntoReposicion = table.Column<int>(type: "int", nullable: false),
                    fechaCreacion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    fechaUltimaAlerta = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    estado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertaStock", x => x.idAlertaStock);
                    table.ForeignKey(
                        name: "FK_AlertaStock_Producto_idProducto",
                        column: x => x.idProducto,
                        principalTable: "Producto",
                        principalColumn: "IdProducto",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AlertaStock_Sede_idSede",
                        column: x => x.idSede,
                        principalTable: "Sede",
                        principalColumn: "idSede",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AlertaStock_idProducto",
                table: "AlertaStock",
                column: "idProducto");

            migrationBuilder.CreateIndex(
                name: "IX_AlertaStock_idSede",
                table: "AlertaStock",
                column: "idSede");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlertaStock");
        }
    }
}
