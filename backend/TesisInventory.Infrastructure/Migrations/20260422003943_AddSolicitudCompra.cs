using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TesisInventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSolicitudCompra : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "SolicitudCompra",
                columns: table => new
                {
                    IdSolicitudCompra = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdProducto = table.Column<int>(type: "int", nullable: false),
                    IdSede = table.Column<int>(type: "int", nullable: false),
                    IdUsuarioSolicitante = table.Column<int>(type: "int", nullable: false),
                    IdUsuarioAprobador = table.Column<int>(type: "int", nullable: true),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    FechaSolicitud = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaDecision = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Observaciones = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MotivoRechazo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitudCompra", x => x.IdSolicitudCompra);
                    table.ForeignKey(
                        name: "FK_SolicitudCompra_Producto_IdProducto",
                        column: x => x.IdProducto,
                        principalTable: "Producto",
                        principalColumn: "IdProducto",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SolicitudCompra_Sede_IdSede",
                        column: x => x.IdSede,
                        principalTable: "Sede",
                        principalColumn: "idSede",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SolicitudCompra_Usuario_IdUsuarioAprobador",
                        column: x => x.IdUsuarioAprobador,
                        principalTable: "Usuario",
                        principalColumn: "idUsuario",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SolicitudCompra_Usuario_IdUsuarioSolicitante",
                        column: x => x.IdUsuarioSolicitante,
                        principalTable: "Usuario",
                        principalColumn: "idUsuario",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudCompra_IdProducto",
                table: "SolicitudCompra",
                column: "IdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudCompra_IdSede",
                table: "SolicitudCompra",
                column: "IdSede");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudCompra_IdUsuarioAprobador",
                table: "SolicitudCompra",
                column: "IdUsuarioAprobador");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudCompra_IdUsuarioSolicitante",
                table: "SolicitudCompra",
                column: "IdUsuarioSolicitante");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SolicitudCompra");
        }
    }
}
