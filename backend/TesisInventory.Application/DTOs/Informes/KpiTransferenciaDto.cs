namespace TesisInventory.Application.DTOs.Informes
{
    public class KpiTransferenciaDto
    {
        public int TotalTransferencias { get; set; }
        public int TotalPrestamos { get; set; }
        public double PorcentajePrestamos { get; set; }
        public double TasaRechazo { get; set; }
        public double TiempoPromedioPrestamoDias { get; set; }
        public string SedeMasActiva { get; set; } = string.Empty;
        public int SedeMasActivaCantidad { get; set; }
    }
}
