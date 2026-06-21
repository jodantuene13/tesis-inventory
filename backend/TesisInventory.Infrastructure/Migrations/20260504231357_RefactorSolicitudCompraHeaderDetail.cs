using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TesisInventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactorSolicitudCompraHeaderDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SolicitudCompra_Producto_IdProducto",
                table: "SolicitudCompra");

            migrationBuilder.DropIndex(
                name: "IX_SolicitudCompra_IdProducto",
                table: "SolicitudCompra");

            migrationBuilder.DropColumn(
                name: "Cantidad",
                table: "SolicitudCompra");

            migrationBuilder.DropColumn(
                name: "IdProducto",
                table: "SolicitudCompra");

            migrationBuilder.AddColumn<string>(
                name: "MotivoSolicitud",
                table: "SolicitudCompra",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "OrdenTrabajo",
                table: "SolicitudCompra",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "TareaARealizar",
                table: "SolicitudCompra",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "TicketSolicitud",
                table: "SolicitudCompra",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SolicitudCompraDetalle",
                columns: table => new
                {
                    IdSolicitudCompraDetalle = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdSolicitudCompra = table.Column<int>(type: "int", nullable: false),
                    IdProducto = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitudCompraDetalle", x => x.IdSolicitudCompraDetalle);
                    table.ForeignKey(
                        name: "FK_SolicitudCompraDetalle_Producto_IdProducto",
                        column: x => x.IdProducto,
                        principalTable: "Producto",
                        principalColumn: "IdProducto",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SolicitudCompraDetalle_SolicitudCompra_IdSolicitudCompra",
                        column: x => x.IdSolicitudCompra,
                        principalTable: "SolicitudCompra",
                        principalColumn: "IdSolicitudCompra",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudCompraDetalle_IdProducto",
                table: "SolicitudCompraDetalle",
                column: "IdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudCompraDetalle_IdSolicitudCompra",
                table: "SolicitudCompraDetalle",
                column: "IdSolicitudCompra");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SolicitudCompraDetalle");

            migrationBuilder.DropColumn(
                name: "MotivoSolicitud",
                table: "SolicitudCompra");

            migrationBuilder.DropColumn(
                name: "OrdenTrabajo",
                table: "SolicitudCompra");

            migrationBuilder.DropColumn(
                name: "TareaARealizar",
                table: "SolicitudCompra");

            migrationBuilder.DropColumn(
                name: "TicketSolicitud",
                table: "SolicitudCompra");

            migrationBuilder.AddColumn<int>(
                name: "Cantidad",
                table: "SolicitudCompra",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdProducto",
                table: "SolicitudCompra",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudCompra_IdProducto",
                table: "SolicitudCompra",
                column: "IdProducto");

            migrationBuilder.AddForeignKey(
                name: "FK_SolicitudCompra_Producto_IdProducto",
                table: "SolicitudCompra",
                column: "IdProducto",
                principalTable: "Producto",
                principalColumn: "IdProducto",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
