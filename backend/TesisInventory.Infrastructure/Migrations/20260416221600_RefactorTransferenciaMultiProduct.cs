using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TesisInventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactorTransferenciaMultiProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transferencia_Producto_idProducto",
                table: "Transferencia");

            migrationBuilder.DropIndex(
                name: "IX_Transferencia_idProducto",
                table: "Transferencia");

            migrationBuilder.DropColumn(
                name: "cantidad",
                table: "Transferencia");

            migrationBuilder.DropColumn(
                name: "idProducto",
                table: "Transferencia");

            migrationBuilder.DropColumn(
                name: "stockOrigenSnapshot",
                table: "Transferencia");

            migrationBuilder.DropColumn(
                name: "Orden",
                table: "FamiliaAtributo");

            migrationBuilder.DropColumn(
                name: "Orden",
                table: "AtributoOpcion");

            migrationBuilder.CreateTable(
                name: "TransferenciaDetalle",
                columns: table => new
                {
                    idTransferenciaDetalle = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    idTransferencia = table.Column<int>(type: "int", nullable: false),
                    idProducto = table.Column<int>(type: "int", nullable: false),
                    cantidad = table.Column<int>(type: "int", nullable: false),
                    stockOrigenSnapshot = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferenciaDetalle", x => x.idTransferenciaDetalle);
                    table.ForeignKey(
                        name: "FK_TransferenciaDetalle_Producto_idProducto",
                        column: x => x.idProducto,
                        principalTable: "Producto",
                        principalColumn: "IdProducto",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferenciaDetalle_Transferencia_idTransferencia",
                        column: x => x.idTransferencia,
                        principalTable: "Transferencia",
                        principalColumn: "idTransferencia",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_TransferenciaDetalle_idProducto",
                table: "TransferenciaDetalle",
                column: "idProducto");

            migrationBuilder.CreateIndex(
                name: "IX_TransferenciaDetalle_idTransferencia",
                table: "TransferenciaDetalle",
                column: "idTransferencia");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransferenciaDetalle");

            migrationBuilder.AddColumn<int>(
                name: "cantidad",
                table: "Transferencia",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "idProducto",
                table: "Transferencia",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "stockOrigenSnapshot",
                table: "Transferencia",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Orden",
                table: "FamiliaAtributo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Orden",
                table: "AtributoOpcion",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transferencia_idProducto",
                table: "Transferencia",
                column: "idProducto");

            migrationBuilder.AddForeignKey(
                name: "FK_Transferencia_Producto_idProducto",
                table: "Transferencia",
                column: "idProducto",
                principalTable: "Producto",
                principalColumn: "IdProducto",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
