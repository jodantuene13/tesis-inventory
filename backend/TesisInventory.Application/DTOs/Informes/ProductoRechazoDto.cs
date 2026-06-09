namespace TesisInventory.Application.DTOs.Informes
{
    public class ProductoRechazoDto
    {
        public string Producto { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public string Familia { get; set; } = string.Empty;
        public int Rechazadas { get; set; }
        public int Total { get; set; }
        public double Indice { get; set; }
        public string MotivoPrincipal { get; set; } = string.Empty;
    }
}
