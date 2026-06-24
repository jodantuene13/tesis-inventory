using System.Collections.Generic;

namespace TesisInventory.Application.DTOs.Sedes
{
    public class StockItemDto
    {
        public string NombreProducto { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public int Cantidad { get; set; }
    }

    public class SedeDeleteBloqueante
    {
        public string Tipo { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public int? Cantidad { get; set; }
        public List<StockItemDto>? Items { get; set; }
    }
}
