using System.Collections.Generic;

namespace TesisInventory.Application.DTOs
{
    public class UpdateUserDto
    {
        public string NombreUsuario { get; set; } = string.Empty;
        public int IdRol { get; set; }
        public int IdSede { get; set; }
        public bool Estado { get; set; }
        public bool TodasLasSedes { get; set; } = false;
        public bool LimitarOperacionSedePrimaria { get; set; } = false;
        public List<int> SedesIds { get; set; } = new List<int>();
    }
}
