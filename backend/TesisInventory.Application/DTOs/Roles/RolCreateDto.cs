using System.Collections.Generic;

namespace TesisInventory.Application.DTOs.Roles
{
    public class RolCreateDto
    {
        public string NombreRol { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public List<int> PermisosIds { get; set; } = new List<int>();
    }
}
