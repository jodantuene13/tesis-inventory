using System.Collections.Generic;

namespace TesisInventory.Application.DTOs.Roles
{
    public class RolUpdateDto
    {
        public int IdRol { get; set; }
        public string NombreRol { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public bool TodasLasSedes { get; set; }
        public bool LimitarOperacionSedePrimaria { get; set; }

        public List<int> PermisosIds { get; set; } = new List<int>();
        public List<int> SedesIds { get; set; } = new List<int>();
    }
}
