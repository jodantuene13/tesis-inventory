namespace TesisInventory.Application.DTOs.Roles
{
    public class PermisoDto
    {
        public int IdPermiso { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Modulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
    }
}
