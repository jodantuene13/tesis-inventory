namespace TesisInventory.Application.DTOs
{
    public class UnidadMedidaDto
    {
        public int IdUnidadMedida { get; set; }
        public string Simbolo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public bool Activo { get; set; }
    }

    public class CreateUnidadMedidaDto
    {
        public string Simbolo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public bool Activo { get; set; } = true;
    }

    public class UpdateUnidadMedidaDto
    {
        public string Simbolo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public bool Activo { get; set; }
    }
}
