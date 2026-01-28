namespace TesisInventory.Application.DTOs
{
    public class UpdateUserDto
    {
        public string NombreUsuario { get; set; } = string.Empty;
        public int IdRol { get; set; }
        public int IdSede { get; set; }
    }
}
