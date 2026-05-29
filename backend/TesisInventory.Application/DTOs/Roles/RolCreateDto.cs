using System.Collections.Generic;

namespace TesisInventory.Application.DTOs.Roles
{
    public class RolCreateDto
    {
        public string NombreRol { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public bool TodasLasSedes { get; set; }
        public bool LimitarOperacionSedePrimaria { get; set; }

        public List<int> PermisosIds { get; set; } = new List<int>();
        public List<int> SedesIds { get; set; } = new List<int>();
    }
}
