using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TesisInventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixTransferenciaCols : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistorialTransferencia_Transferencia_IdTransferencia",
                table: "HistorialTransferencia");

            migrationBuilder.DropForeignKey(
                name: "FK_HistorialTransferencia_Usuario_IdUsuario",
                table: "HistorialTransferencia");

            migrationBuilder.DropForeignKey(
                name: "FK_Transferencia_Producto_IdProducto",
                table: "Transferencia");

            migrationBuilder.DropForeignKey(
                name: "FK_Transferencia_Sede_IdSedeDestino",
                table: "Transferencia");

            migrationBuilder.DropForeignKey(
                name: "FK_Transferencia_Sede_IdSedeOrigen",
                table: "Transferencia");

            migrationBuilder.DropForeignKey(
                name: "FK_Transferencia_Usuario_IdUsuarioSolicita",
                table: "Transferencia");

            migrationBuilder.RenameColumn(
                name: "Observaciones",
                table: "Transferencia",
                newName: "observaciones");

            migrationBuilder.RenameColumn(
                name: "IdUsuarioSolicita",
                table: "Transferencia",
                newName: "idUsuarioSolicita");

            migrationBuilder.RenameColumn(
                name: "IdSedeOrigen",
                table: "Transferencia",
                newName: "idSedeOrigen");

            migrationBuilder.RenameColumn(
                name: "IdSedeDestino",
                table: "Transferencia",
                newName: "idSedeDestino");

            migrationBuilder.RenameColumn(
                name: "IdProducto",
                table: "Transferencia",
                newName: "idProducto");

            migrationBuilder.RenameColumn(
                name: "FechaSolicitud",
                table: "Transferencia",
                newName: "fechaSolicitud");

            migrationBuilder.RenameColumn(
                name: "Estado",
                table: "Transferencia",
                newName: "estado");

            migrationBuilder.RenameColumn(
                name: "Cantidad",
                table: "Transferencia",
                newName: "cantidad");

            migrationBuilder.RenameColumn(
                name: "IdTransferencia",
                table: "Transferencia",
                newName: "idTransferencia");

            migrationBuilder.RenameIndex(
                name: "IX_Transferencia_IdUsuarioSolicita",
                table: "Transferencia",
                newName: "IX_Transferencia_idUsuarioSolicita");

            migrationBuilder.RenameIndex(
                name: "IX_Transferencia_IdSedeOrigen",
                table: "Transferencia",
                newName: "IX_Transferencia_idSedeOrigen");

            migrationBuilder.RenameIndex(
                name: "IX_Transferencia_IdSedeDestino",
                table: "Transferencia",
                newName: "IX_Transferencia_idSedeDestino");

            migrationBuilder.RenameIndex(
                name: "IX_Transferencia_IdProducto",
                table: "Transferencia",
                newName: "IX_Transferencia_idProducto");

            migrationBuilder.RenameColumn(
                name: "Observaciones",
                table: "HistorialTransferencia",
                newName: "observaciones");

            migrationBuilder.RenameColumn(
                name: "IdUsuario",
                table: "HistorialTransferencia",
                newName: "idUsuario");

            migrationBuilder.RenameColumn(
                name: "IdTransferencia",
                table: "HistorialTransferencia",
                newName: "idTransferencia");

            migrationBuilder.RenameColumn(
                name: "Fecha",
                table: "HistorialTransferencia",
                newName: "fecha");

            migrationBuilder.RenameColumn(
                name: "EstadoNuevo",
                table: "HistorialTransferencia",
                newName: "estadoNuevo");

            migrationBuilder.RenameColumn(
                name: "EstadoAnterior",
                table: "HistorialTransferencia",
                newName: "estadoAnterior");

            migrationBuilder.RenameColumn(
                name: "IdHistorialTransferencia",
                table: "HistorialTransferencia",
                newName: "idHistorialTransferencia");

            migrationBuilder.RenameIndex(
                name: "IX_HistorialTransferencia_IdUsuario",
                table: "HistorialTransferencia",
                newName: "IX_HistorialTransferencia_idUsuario");

            migrationBuilder.RenameIndex(
                name: "IX_HistorialTransferencia_IdTransferencia",
                table: "HistorialTransferencia",
                newName: "IX_HistorialTransferencia_idTransferencia");

            migrationBuilder.AddColumn<int>(
                name: "motivo",
                table: "Transferencia",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_HistorialTransferencia_Transferencia_idTransferencia",
                table: "HistorialTransferencia",
                column: "idTransferencia",
                principalTable: "Transferencia",
                principalColumn: "idTransferencia",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HistorialTransferencia_Usuario_idUsuario",
                table: "HistorialTransferencia",
                column: "idUsuario",
                principalTable: "Usuario",
                principalColumn: "idUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transferencia_Producto_idProducto",
                table: "Transferencia",
                column: "idProducto",
                principalTable: "Producto",
                principalColumn: "IdProducto",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transferencia_Sede_idSedeDestino",
                table: "Transferencia",
                column: "idSedeDestino",
                principalTable: "Sede",
                principalColumn: "idSede",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transferencia_Sede_idSedeOrigen",
                table: "Transferencia",
                column: "idSedeOrigen",
                principalTable: "Sede",
                principalColumn: "idSede",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transferencia_Usuario_idUsuarioSolicita",
                table: "Transferencia",
                column: "idUsuarioSolicita",
                principalTable: "Usuario",
                principalColumn: "idUsuario",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistorialTransferencia_Transferencia_idTransferencia",
                table: "HistorialTransferencia");

            migrationBuilder.DropForeignKey(
                name: "FK_HistorialTransferencia_Usuario_idUsuario",
                table: "HistorialTransferencia");

            migrationBuilder.DropForeignKey(
                name: "FK_Transferencia_Producto_idProducto",
                table: "Transferencia");

            migrationBuilder.DropForeignKey(
                name: "FK_Transferencia_Sede_idSedeDestino",
                table: "Transferencia");

            migrationBuilder.DropForeignKey(
                name: "FK_Transferencia_Sede_idSedeOrigen",
                table: "Transferencia");

            migrationBuilder.DropForeignKey(
                name: "FK_Transferencia_Usuario_idUsuarioSolicita",
                table: "Transferencia");

            migrationBuilder.DropColumn(
                name: "motivo",
                table: "Transferencia");

            migrationBuilder.RenameColumn(
                name: "observaciones",
                table: "Transferencia",
                newName: "Observaciones");

            migrationBuilder.RenameColumn(
                name: "idUsuarioSolicita",
                table: "Transferencia",
                newName: "IdUsuarioSolicita");

            migrationBuilder.RenameColumn(
                name: "idSedeOrigen",
                table: "Transferencia",
                newName: "IdSedeOrigen");

            migrationBuilder.RenameColumn(
                name: "idSedeDestino",
                table: "Transferencia",
                newName: "IdSedeDestino");

            migrationBuilder.RenameColumn(
                name: "idProducto",
                table: "Transferencia",
                newName: "IdProducto");

            migrationBuilder.RenameColumn(
                name: "fechaSolicitud",
                table: "Transferencia",
                newName: "FechaSolicitud");

            migrationBuilder.RenameColumn(
                name: "estado",
                table: "Transferencia",
                newName: "Estado");

            migrationBuilder.RenameColumn(
                name: "cantidad",
                table: "Transferencia",
                newName: "Cantidad");

            migrationBuilder.RenameColumn(
                name: "idTransferencia",
                table: "Transferencia",
                newName: "IdTransferencia");

            migrationBuilder.RenameIndex(
                name: "IX_Transferencia_idUsuarioSolicita",
                table: "Transferencia",
                newName: "IX_Transferencia_IdUsuarioSolicita");

            migrationBuilder.RenameIndex(
                name: "IX_Transferencia_idSedeOrigen",
                table: "Transferencia",
                newName: "IX_Transferencia_IdSedeOrigen");

            migrationBuilder.RenameIndex(
                name: "IX_Transferencia_idSedeDestino",
                table: "Transferencia",
                newName: "IX_Transferencia_IdSedeDestino");

            migrationBuilder.RenameIndex(
                name: "IX_Transferencia_idProducto",
                table: "Transferencia",
                newName: "IX_Transferencia_IdProducto");

            migrationBuilder.RenameColumn(
                name: "observaciones",
                table: "HistorialTransferencia",
                newName: "Observaciones");

            migrationBuilder.RenameColumn(
                name: "idUsuario",
                table: "HistorialTransferencia",
                newName: "IdUsuario");

            migrationBuilder.RenameColumn(
                name: "idTransferencia",
                table: "HistorialTransferencia",
                newName: "IdTransferencia");

            migrationBuilder.RenameColumn(
                name: "fecha",
                table: "HistorialTransferencia",
                newName: "Fecha");

            migrationBuilder.RenameColumn(
                name: "estadoNuevo",
                table: "HistorialTransferencia",
                newName: "EstadoNuevo");

            migrationBuilder.RenameColumn(
                name: "estadoAnterior",
                table: "HistorialTransferencia",
                newName: "EstadoAnterior");

            migrationBuilder.RenameColumn(
                name: "idHistorialTransferencia",
                table: "HistorialTransferencia",
                newName: "IdHistorialTransferencia");

            migrationBuilder.RenameIndex(
                name: "IX_HistorialTransferencia_idUsuario",
                table: "HistorialTransferencia",
                newName: "IX_HistorialTransferencia_IdUsuario");

            migrationBuilder.RenameIndex(
                name: "IX_HistorialTransferencia_idTransferencia",
                table: "HistorialTransferencia",
                newName: "IX_HistorialTransferencia_IdTransferencia");

            migrationBuilder.AddForeignKey(
                name: "FK_HistorialTransferencia_Transferencia_IdTransferencia",
                table: "HistorialTransferencia",
                column: "IdTransferencia",
                principalTable: "Transferencia",
                principalColumn: "IdTransferencia",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HistorialTransferencia_Usuario_IdUsuario",
                table: "HistorialTransferencia",
                column: "IdUsuario",
                principalTable: "Usuario",
                principalColumn: "idUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transferencia_Producto_IdProducto",
                table: "Transferencia",
                column: "IdProducto",
                principalTable: "Producto",
                principalColumn: "IdProducto",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transferencia_Sede_IdSedeDestino",
                table: "Transferencia",
                column: "IdSedeDestino",
                principalTable: "Sede",
                principalColumn: "idSede",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transferencia_Sede_IdSedeOrigen",
                table: "Transferencia",
                column: "IdSedeOrigen",
                principalTable: "Sede",
                principalColumn: "idSede",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transferencia_Usuario_IdUsuarioSolicita",
                table: "Transferencia",
                column: "IdUsuarioSolicita",
                principalTable: "Usuario",
                principalColumn: "idUsuario",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
