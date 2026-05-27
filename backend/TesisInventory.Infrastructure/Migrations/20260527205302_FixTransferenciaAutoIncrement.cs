using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TesisInventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixTransferenciaAutoIncrement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "idTransferencia",
                table: "Transferencia",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", Microsoft.EntityFrameworkCore.Metadata.MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "idHistorialTransferencia",
                table: "HistorialTransferencia",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", Microsoft.EntityFrameworkCore.Metadata.MySqlValueGenerationStrategy.IdentityColumn);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "idTransferencia",
                table: "Transferencia",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", Microsoft.EntityFrameworkCore.Metadata.MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "idHistorialTransferencia",
                table: "HistorialTransferencia",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", Microsoft.EntityFrameworkCore.Metadata.MySqlValueGenerationStrategy.IdentityColumn);
        }
    }
}
