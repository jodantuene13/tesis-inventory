using System;

namespace TesisInventory.Application.DTOs.Informes
{
    public class ProductoInmovilizadoDto
    {
        public int IdProducto { get; set; }
        public string Producto { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public string Familia { get; set; } = string.Empty;
        public string Sede { get; set; } = string.Empty;
        public int StockActual { get; set; }
        public DateTime? UltimoIngreso { get; set; }
        public int DiasSinEgreso { get; set; }
    }
}
