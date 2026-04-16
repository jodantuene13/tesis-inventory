namespace TesisInventory.Application.DTOs.Stock
{
    public class TransferenciaDetalleDto
    {
        public int IdTransferenciaDetalle { get; set; }
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public int? StockOrigenSnapshot { get; set; }
    }
}
