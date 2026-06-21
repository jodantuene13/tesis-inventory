using System.Collections.Generic;

namespace TesisInventory.Application.DTOs.Informes
{
    /// <summary>
    /// Contenedor principal del informe de Rotación de Productos.
    /// Agrupa los tres rankings en una sola respuesta.
    /// </summary>
    public class InformeRotacionDto
    {
        /// <summary>Ranking principal por índice de rotación (Top N).</summary>
        public List<ProductoRotacionDto> Rotacion { get; set; } = new();

        /// <summary>Ranking completo de productos que más ingresaron (ENTRADA + TRANSFERENCIA).</summary>
        public List<ProductoMovimientoDto> MayorIngreso { get; set; } = new();

        /// <summary>Ranking completo de productos que más egresaron (SALIDA + CONSUMO).</summary>
        public List<ProductoMovimientoDto> MayorEgreso { get; set; } = new();

        // ── Metadatos del período ──────────────────────────────────────────────
        public string FechaDesde { get; set; } = string.Empty;
        public string FechaHasta { get; set; } = string.Empty;
        public int TotalDiasPeriodo { get; set; }

        // ── KPIs globales ──────────────────────────────────────────────────────
        /// <summary>Índice de rotación promedio de todos los productos del período.</summary>
        public double RotacionPromedio { get; set; }

        /// <summary>Saldo neto total del período (total ingresos - total egresos).</summary>
        public int SaldoNetoTotal { get; set; }
    }
}
