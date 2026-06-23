using System;

namespace TesisInventory.Application.DTOs.Informes
{
    /// <summary>
    /// DTO para los rankings desagregados de Ingresos y Egresos.
    /// </summary>
    public class ProductoMovimientoDto
    {
        public int IdProducto { get; set; }
        public string Producto { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public string Familia { get; set; } = string.Empty;
        public string Sede { get; set; } = string.Empty;

        public string UnidadMedida { get; set; } = "u.";

        /// <summary>Total de unidades movidas en el tipo de movimiento correspondiente.</summary>
        public int TotalUnidades { get; set; }

        /// <summary>Cantidad de operaciones/movimientos distintos registrados.</summary>
        public int CantidadOperaciones { get; set; }

        public DateTime? UltimaFecha { get; set; }
    }
}
