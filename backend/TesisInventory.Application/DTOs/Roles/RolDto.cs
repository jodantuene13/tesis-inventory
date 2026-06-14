using System.Collections.Generic;

namespace TesisInventory.Application.DTOs.Roles
{
    public class RolDto
    {
        public int IdRol { get; set; }
        public string NombreRol { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public List<int> PermisosIds { get; set; } = new List<int>();
    }
}
