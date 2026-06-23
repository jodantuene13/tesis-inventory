using System;

namespace TesisInventory.Application.DTOs.Informes
{
    /// <summary>
    /// Datos de rotación por producto/sede para el ranking principal.
    /// Índice = TotalEgresos / StockPromedioPonderado (salidas / stock promedio).
    /// </summary>
    public class ProductoRotacionDto
    {
        public int IdProducto { get; set; }
        public string Producto { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public string Familia { get; set; } = string.Empty;
        public string Sede { get; set; } = string.Empty;

        public string UnidadMedida { get; set; } = "u.";

        /// <summary>Total de unidades ingresadas (ENTRADA + TRANSFERENCIA) en el período.</summary>
        public int TotalIngresos { get; set; }

        /// <summary>Total de unidades egresadas (SALIDA + CONSUMO) en el período.</summary>
        public int TotalEgresos { get; set; }

        /// <summary>Stock promedio ponderado por tiempo Σ(Stockᵢ × Díasᵢ) / días_total.</summary>
        public double StockPromedioPonderado { get; set; }

        /// <summary>Índice de rotación = TotalEgresos / StockPromedioPonderado.</summary>
        public double IndiceRotacion { get; set; }

        /// <summary>Saldo neto del período (ingresos - egresos).</summary>
        public int SaldoNeto => TotalIngresos - TotalEgresos;

        public DateTime? UltimoIngreso { get; set; }
        public DateTime? UltimoEgreso { get; set; }

        /// <summary>Alta / Media / Baja según índice de rotación comparado con el promedio.</summary>
        public string Tendencia { get; set; } = string.Empty;
    }
}
