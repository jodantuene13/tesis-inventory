namespace TesisInventory.Application.DTOs.Informes
{
    public class FamiliaConsumoDto
    {
        public string Familia { get; set; } = string.Empty;
        public int TotalIngresos { get; set; }
        public int TotalEgresos { get; set; }
        public double RatioConsumo { get; set; }
        public int CantidadProductos { get; set; }
    }
}
