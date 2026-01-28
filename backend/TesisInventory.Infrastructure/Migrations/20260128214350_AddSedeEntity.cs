using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TesisInventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSedeEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            // Rol table already exists, skipping creation
            /*
            migrationBuilder.CreateTable(
                name: "Rol",
                columns: table => new
                {
                    idRol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombreRol = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    descripcion = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rol", x => x.idRol);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
            */

            migrationBuilder.CreateTable(
                name: "Sede",
                columns: table => new
                {
                    idSede = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombreSede = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sede", x => x.idSede);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            // Usuario table exists, adding idSede column
            /*
            migrationBuilder.CreateTable(
                name: "Usuario",
                ...
            */
            
            migrationBuilder.AddColumn<int>(
                name: "idSede",
                table: "Usuario",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Removing FK creation since table isn't being created and FK definition was part of CreateTable usually, 
            // or if we needed to AddForeignKey, we would do it here if nav prop existed.
            // Since nav prop Sede is commented out in Entity, no FK needed.

            /*
            migrationBuilder.CreateIndex(
                name: "IX_Usuario_idRol",
                table: "Usuario",
                column: "idRol");
            */
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sede");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Rol");
        }
    }
}
