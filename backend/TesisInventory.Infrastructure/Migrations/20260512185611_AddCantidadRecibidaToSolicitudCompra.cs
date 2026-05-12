using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TesisInventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCantidadRecibidaToSolicitudCompra : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CantidadRecibida",
                table: "SolicitudCompraDetalle",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CantidadRecibida",
                table: "SolicitudCompraDetalle");
        }
    }
}
