using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TesisInventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOperacionStockEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdOperacion",
                table: "Movimiento",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OperacionStock",
                columns: table => new
                {
                    idOperacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    idSede = table.Column<int>(type: "int", nullable: false),
                    idUsuario = table.Column<int>(type: "int", nullable: false),
                    tipoOperacion = table.Column<int>(type: "int", nullable: false),
                    fecha = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    motivo = table.Column<int>(type: "int", nullable: false),
                    ordenTrabajo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ordenCompra = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ticketSolicitud = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    observaciones = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperacionStock", x => x.idOperacion);
                    table.ForeignKey(
                        name: "FK_OperacionStock_Sede_idSede",
                        column: x => x.idSede,
                        principalTable: "Sede",
                        principalColumn: "idSede",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OperacionStock_Usuario_idUsuario",
                        column: x => x.idUsuario,
                        principalTable: "Usuario",
                        principalColumn: "idUsuario",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Movimiento_IdOperacion",
                table: "Movimiento",
                column: "IdOperacion");

            migrationBuilder.CreateIndex(
                name: "IX_OperacionStock_idSede",
                table: "OperacionStock",
                column: "idSede");

            migrationBuilder.CreateIndex(
                name: "IX_OperacionStock_idUsuario",
                table: "OperacionStock",
                column: "idUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Movimiento_OperacionStock_IdOperacion",
                table: "Movimiento",
                column: "IdOperacion",
                principalTable: "OperacionStock",
                principalColumn: "idOperacion",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movimiento_OperacionStock_IdOperacion",
                table: "Movimiento");

            migrationBuilder.DropTable(
                name: "OperacionStock");

            migrationBuilder.DropIndex(
                name: "IX_Movimiento_IdOperacion",
                table: "Movimiento");

            migrationBuilder.DropColumn(
                name: "IdOperacion",
                table: "Movimiento");
        }
    }
}
