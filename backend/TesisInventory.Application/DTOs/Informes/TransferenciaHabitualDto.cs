namespace TesisInventory.Application.DTOs.Informes
{
    public class TransferenciaHabitualDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string? Sku { get; set; }
        public string? Familia { get; set; }
        public int Cantidad { get; set; }
        public double Porcentaje { get; set; }
    }
}
